using UnityEngine;

public class MonkeyMovement : MonoBehaviour, IAnimalAbility
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float turnSpeed = 12f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    private bool isGrounded = true;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Roll")]
    public float rollSpeed = 12f;
    public float rollDuration = 0.5f;
    public float rollCooldown = 1.2f;

    private float rollTimer;
    private float lastRollTime;
    private bool isRolling = false;
    private Vector3 rollDirection;

    [Header("Water")]
    public float sinkForce = 6f;
    private bool isInWater = false;

    [Header("Animation")]
    public float moveThreshold = 0.1f;

    private Animator anim;
    private Rigidbody rb;
    private Camera cam;

    public MonoBehaviour playerMovement;
    public MonoBehaviour cameraLook;

    // INPUT
    private Vector3 moveInput;
    private bool jumpPressed;
    private bool rollPressed;
    private bool runHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        FindCamera();
    }

    void Update()
    {
        // Reconnect camera if prefab was spawned/swapped
        if (cam == null)
        {
            FindCamera();

            if (cam == null)
                return;
        }

        ReadInput();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (cam == null)
            return;

        Move();
        HandleRoll();
        HandleJump();
        HandleJumpPhysics();
        HandleWater();
    }

    void FindCamera()
    {
        if (Camera.main != null)
        {
            cam = Camera.main;
        }
    }

    void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        moveInput = (camForward * v + camRight * h).normalized;

        runHeld = Input.GetKey(KeyCode.LeftShift);

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;

        if (Input.GetKeyDown(KeyCode.LeftControl))
            rollPressed = true;
    }

    void Move()
    {
        if (isRolling)
            return;

        float speed = runHeld ? runSpeed : walkSpeed;

        if (isInWater)
        {
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );
        }
        else
        {
            Vector3 velocity = moveInput * speed;

            rb.linearVelocity = new Vector3(
                velocity.x,
                rb.linearVelocity.y,
                velocity.z
            );
        }

        // Rotate only when moving
        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveInput, Vector3.up);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );
        }

        if (rollPressed)
        {
            TryRoll();
            rollPressed = false;
        }
    }

    void HandleJump()
    {
        if (!jumpPressed)
            return;

        jumpPressed = false;

        if (!isGrounded || isInWater || !rb.useGravity)
            return;

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );

        isGrounded = false;

        anim.SetTrigger("Jump");
    }

    void TryRoll()
    {
        if (!isGrounded)
            return;

        if (Time.time < lastRollTime + rollCooldown)
            return;

        isRolling = true;
        rollTimer = rollDuration;
        lastRollTime = Time.time;

        rollDirection =
            moveInput.sqrMagnitude > 0.01f
            ? moveInput
            : transform.forward;

        anim.SetTrigger("Roll");
    }

    void HandleRoll()
    {
        if (!isRolling)
            return;

        rollTimer -= Time.fixedDeltaTime;

        rb.linearVelocity = new Vector3(
            rollDirection.x * rollSpeed,
            rb.linearVelocity.y,
            rollDirection.z * rollSpeed
        );

        if (rollTimer <= 0f)
        {
            isRolling = false;
        }
    }

    void HandleJumpPhysics()
    {
        if (isInWater)
            return;

        // Faster falling
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity +=
                Vector3.up *
                Physics.gravity.y *
                (fallMultiplier - 1f) *
                Time.fixedDeltaTime;
        }
        // Short hop when releasing jump
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity +=
                Vector3.up *
                Physics.gravity.y *
                (lowJumpMultiplier - 1f) *
                Time.fixedDeltaTime;
        }
    }

    void HandleWater()
    {
        if (!isInWater)
            return;

        rb.AddForce(
            Vector3.down * sinkForce,
            ForceMode.Acceleration
        );
    }

    void UpdateAnimations()
    {
        Vector3 horizontalVel =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        float animSpeed = horizontalVel.magnitude;

        anim.SetFloat(
            "Speed",
            animSpeed < moveThreshold ? 0f : animSpeed
        );

        anim.SetBool("isGrounded", isGrounded);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.linearDamping = 2f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.linearDamping = 0f;
        }
    }

    public void OnFormActivated()
    {
        enabled = true;
    }

    public void OnFormDeactivated()
    {
        enabled = false;
    }
}