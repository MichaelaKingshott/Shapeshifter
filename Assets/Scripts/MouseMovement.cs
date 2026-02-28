using UnityEngine;

public class MouseMovement : MonoBehaviour, IAnimalAbility
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float turnSpeed = 12f;

    [Header("Jumping")]
    public float jumpForce = 8f;
    private bool isGrounded = true;

    [Header("Jump Feel")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Water")]
    public float sinkForce = 6f;
    private bool isInWater = false;

    [Header("Animation")]
    public float moveThreshold = 0.1f; // minimum speed to trigger walking
    private Animator anim;


    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // --- CAMERA-RELATIVE MOVEMENT ---
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = camForward * v + camRight * h;

        // Movement (XZ)
        rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);

        // Rotation (only if moving)
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Jump (disabled in water)
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isInWater)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Apply better jump physics (disabled in water)
        if (!isInWater)
        {
            if (rb.linearVelocity.y < 0)
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }

        // Sink in water
        if (isInWater)
        {
            rb.AddForce(Vector3.down * sinkForce, ForceMode.Acceleration);
        }

        // Horizontal speed for walking animation
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = horizontalVel.magnitude;
        anim.SetFloat("Speed", speed < moveThreshold ? 0f : speed);

        // Jumping animation
        anim.SetBool("IsJumping", !isGrounded);
    }

    // Ground check
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Water check
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.linearDamping = 2f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.linearDamping = 0f;
        }
    }

    public void OnFormActivated() => this.enabled = true;
    public void OnFormDeactivated() => this.enabled = false;
}





