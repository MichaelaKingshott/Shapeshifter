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

    public MonoBehaviour playerMovement;
    public MonoBehaviour cameraLook;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = camForward * v + camRight * h;

        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        if (!isRolling)
        {
            if (isInWater)
            {
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
            else
            {
                rb.linearVelocity = new Vector3(move.x * speed, rb.linearVelocity.y, move.z * speed);
            }

            if (move != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !isInWater && rb.useGravity)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
                anim.SetTrigger("Jump");
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                TryRoll(move);
            }
        }

        HandleRoll();
        HandleJumpPhysics();
        HandleWater();

        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float animSpeed = horizontalVel.magnitude;

        anim.SetFloat("Speed", animSpeed < moveThreshold ? 0f : animSpeed);
        anim.SetBool("isGrounded", isGrounded);
    }

    void TryRoll(Vector3 move)
    {
        if (!isGrounded || Time.time < lastRollTime + rollCooldown)
            return;

        isRolling = true;
        rollTimer = rollDuration;
        lastRollTime = Time.time;

        rollDirection = move != Vector3.zero ? move.normalized : transform.forward;

        anim.SetTrigger("Roll");
    }

    void HandleRoll()
    {
        if (!isRolling) return;

        rollTimer -= Time.deltaTime;

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
        if (isInWater) return;

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.linearVelocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void HandleWater()
    {
        if (isInWater)
        {
            rb.AddForce(Vector3.down * sinkForce, ForceMode.Acceleration);
        }
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

    public void OnFormActivated() => enabled = true;
    public void OnFormDeactivated() => enabled = false;
}