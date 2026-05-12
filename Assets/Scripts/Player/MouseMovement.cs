using UnityEngine;

public class MouseMovement : MonoBehaviour, IAnimalAbility
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float turnSpeed = 12f;

    [Header("Jumping")]
    public float jumpForce = 8f;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Water")]
    public float sinkForce = 6f;

    [Header("Animation")]
    public float moveThreshold = 0.1f;

    private Rigidbody rb;
    private Animator anim;
    private Camera cam;

    private bool isGrounded = true;
    private bool isInWater = false;

    private Vector3 moveInput;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        FindCamera();
    }

    void Update()
    {
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
        Move();
        HandleJump();
        HandleJumpPhysics();
        HandleWater();
    }

    void FindCamera()
    {
        if (Camera.main != null)
            cam = Camera.main;
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

        moveInput =
            (camForward * v + camRight * h).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;
    }

    void Move()
    {
        if (isInWater)
        {
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );

            return;
        }

        rb.linearVelocity = new Vector3(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y,
            moveInput.z * moveSpeed
        );

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveInput);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );
        }
    }

    void HandleJump()
    {
        if (!jumpPressed)
            return;

        jumpPressed = false;

        if (!isGrounded || isInWater)
            return;

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );

        isGrounded = false;

        anim.SetTrigger("Jump");
    }

    void HandleJumpPhysics()
    {
        if (isInWater)
            return;

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity +=
                Vector3.up *
                Physics.gravity.y *
                (fallMultiplier - 1f) *
                Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 &&
                 !Input.GetKey(KeyCode.Space))
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

        float speed = horizontalVel.magnitude;

        anim.SetFloat(
            "Speed",
            speed < moveThreshold ? 0f : speed
        );

        anim.SetBool("isGrounded", isGrounded);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
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

    public void OnFormActivated() => enabled = true;
    public void OnFormDeactivated() => enabled = false;
}




