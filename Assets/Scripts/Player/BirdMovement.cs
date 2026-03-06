using UnityEngine;

public class BirdMovement : MonoBehaviour, IAnimalAbility
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float flySpeed = 6f;
    public float jumpForce = 8f;
    public float glideFallSpeed = 2f;
    public float turnSpeed = 10f;
    public float glideDuration = 2f;
    public float sinkForce = 5f;

    [Header("Stamina Settings")]
    public float stamina = 5f;
    public float maxStamina = 5f;
    public float staminaRegenRate = 1f;
    public float groundedStaminaRegenBonus = 2f;

    [Header("Animation")]
    public float moveThreshold = 0.1f;
    private Animator anim;

    private Rigidbody rb;
    private bool isGrounded = false;
    private bool isGliding = false;
    private float glideTimer = 0f;
    private bool isInWater = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJumpAndGlide();
        HandleStamina();
        UpdateAnimation();

        if (isInWater)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0); // stop horizontal movement
            rb.AddForce(Vector3.down * sinkForce, ForceMode.Acceleration); // sink
        }
    }

    private void HandleMovement()
    {
        if (isInWater) return; // no horizontal movement in water

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward; camForward.y = 0f; camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right; camRight.y = 0f; camRight.Normalize();
        Vector3 move = (camForward * v + camRight * h).normalized;

        if (isGrounded)
        {
            Vector3 velocity = move * walkSpeed;
            velocity.y = rb.linearVelocity.y;
            rb.linearVelocity = velocity;
        }
        else
        {
            Vector3 horizontalVel = move * flySpeed;
            rb.linearVelocity = new Vector3(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);
        }

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }
    }

    private void HandleJumpAndGlide()
    {
        if (isInWater) return; // cannot jump/fly in water

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && stamina > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            isGrounded = false;
            isGliding = true;
            glideTimer = glideDuration;
            stamina -= 1f;
        }

        if (isGliding && !isGrounded)
        {
            glideTimer -= Time.deltaTime;
            if (rb.linearVelocity.y < -glideFallSpeed)
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, -glideFallSpeed, rb.linearVelocity.z);
            if (glideTimer <= 0) isGliding = false;
        }
    }

    private void HandleStamina()
    {
        float regenRate = isGrounded ? staminaRegenRate + groundedStaminaRegenBonus : staminaRegenRate;
        stamina = Mathf.Min(stamina + regenRate * Time.deltaTime, maxStamina);
    }

    private void UpdateAnimation()
    {
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = horizontalVel.magnitude;
        anim.SetFloat("Speed", speed < moveThreshold ? 0f : speed);

        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isGliding", isGliding);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            isGliding = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
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

    public void OnFormActivated()
    {
        stamina = maxStamina;
        this.enabled = true;
    }

    public void OnFormDeactivated() => this.enabled = false;
}