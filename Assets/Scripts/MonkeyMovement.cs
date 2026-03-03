using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MonkeyMovement : MonoBehaviour
{
    enum MovementState { Normal, Rolling }
    MovementState currentState = MovementState.Normal;

    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;
    public float gravity = -20f;
    public float jumpForce = 8f;

    [Header("Water")]
    public float sinkForce = 6f; // downward speed in water
    private bool isInWater = false;

    [Header("References")]
    public Transform cameraTransform;
    public Animator animator;

    [Header("Roll")]
    public float rollSpeed = 12f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 1.2f;

    CharacterController controller;
    Vector3 velocity;
    Vector3 moveDirection;
    bool grounded;

    float rollTimer;
    float lastRollTime;
    Vector3 rollDirection;

    float coyoteTime = 0.15f;
    float coyoteTimer;
    float jumpBuffer = 0.15f;
    float jumpBufferTimer;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        GroundCheck();
        HandleJumpBuffer();

        switch (currentState)
        {
            case MovementState.Normal:
                HandleMovement();
                break;
            case MovementState.Rolling:
                RollMove();
                break;
        }

        // Sink in water
        if (isInWater)
        {
            velocity.y = Mathf.Max(velocity.y - sinkForce * Time.deltaTime, -sinkForce);
        }
    }

    void GroundCheck()
    {
        grounded = controller.isGrounded;
        if (grounded) coyoteTimer = coyoteTime;
        else coyoteTimer -= Time.deltaTime;

        if (grounded && velocity.y < 0)
            velocity.y = -2f;
    }

    void HandleJumpBuffer()
    {
        if (Input.GetButtonDown("Jump"))
            jumpBufferTimer = jumpBuffer;
        else
            jumpBufferTimer -= Time.deltaTime;
    }

    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0f, v).normalized;

        if (input.magnitude > 0.1f)
        {
            float targetAngle =
                Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg +
                cameraTransform.eulerAngles.y;

            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                Quaternion.Euler(0f, targetAngle, 0f),
                rotationSpeed * Time.deltaTime
            );

            moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }
        else
        {
            moveDirection = Vector3.zero;
        }

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        Vector3 finalMove;

        if (isInWater)
        {
            // Stop horizontal movement in water
            moveDirection = Vector3.zero;
            finalMove = new Vector3(0, velocity.y, 0);
        }
        else
        {
            if (jumpBufferTimer > 0 && coyoteTimer > 0)
            {
                velocity.y = jumpForce;
                jumpBufferTimer = 0;
                coyoteTimer = 0;
                animator?.SetTrigger("Jump");
            }

            velocity.y += gravity * Time.deltaTime;
            finalMove = moveDirection * speed + velocity;
        }

        controller.Move(finalMove * Time.deltaTime);
        animator?.SetFloat("Speed", input.magnitude, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            TryRoll();
    }

    void TryRoll()
    {
        if (!grounded || Time.time < lastRollTime + rollCooldown)
            return;

        currentState = MovementState.Rolling;
        rollTimer = rollDuration;
        lastRollTime = Time.time;

        rollDirection = moveDirection.magnitude > 0.1f
            ? moveDirection
            : transform.forward;

        animator?.SetTrigger("Roll");
    }

    void RollMove()
    {
        rollTimer -= Time.deltaTime;

        if (!isInWater)
            velocity.y += gravity * Time.deltaTime;

        Vector3 move = rollDirection * rollSpeed;
        move.y = velocity.y;

        controller.Move(move * Time.deltaTime);

        if (rollTimer <= 0f)
            currentState = MovementState.Normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = false;
    }
}