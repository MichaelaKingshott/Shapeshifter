using UnityEngine;

public class SquidMovement : MonoBehaviour, IAnimalAbility
{
    [Header("References")]
    public SquidStaminaUI staminaUI;

    [Header("General")]
    public float turnSpeed = 6f;

    [Header("Swimming")]
    public float swimSpeed = 6f;
    public float swimBurstForce = 6f;
    public float waterDrag = 3f;

    [Header("Swim Spam")]
    public float swimSpamDelay = 0.12f;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrain = 1f;
    public float staminaRegen = 2f;

    [Header("Walking")]
    public float walkSpeed = 3f;
    public float jumpForce = 5f;

    [Header("Animation")]
    public float moveThreshold = 0.1f;

    private Rigidbody rb;
    private Animator anim;
    private Camera cam;

    private bool isInWater = false;
    private bool isGrounded = false;

    private float stamina;
    private float lastSwimPress;

    private Vector3 moveInput;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        stamina = maxStamina;

        FindCamera();

        if (staminaUI == null)
            staminaUI = FindFirstObjectByType<SquidStaminaUI>();

        if (staminaUI != null)
            staminaUI.UpdateStamina(1f);
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
        if (isInWater)
            HandleSwimming();
        else
            HandleWalking();
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

    void HandleSwimming()
    {
        Vector3 velocity = rb.linearVelocity;

        if (stamina > 0)
        {
            velocity.x = moveInput.x * swimSpeed;
            velocity.z = moveInput.z * swimSpeed;
        }
        else
        {
            velocity.x = 0f;
            velocity.z = 0f;
        }

        rb.linearVelocity = velocity;

        if (jumpPressed &&
            stamina > 0 &&
            Time.time > lastSwimPress + swimSpamDelay)
        {
            rb.AddForce(
                Vector3.up * swimBurstForce,
                ForceMode.Impulse
            );

            stamina -= staminaDrain;

            lastSwimPress = Time.time;
        }

        jumpPressed = false;

        if (moveInput.sqrMagnitude < 0.01f)
        {
            stamina += staminaRegen * Time.fixedDeltaTime;
        }

        stamina = Mathf.Clamp(
            stamina,
            0f,
            maxStamina
        );

        if (staminaUI != null)
        {
            staminaUI.UpdateStamina(
                stamina / maxStamina
            );
        }

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

    void HandleWalking()
    {
        Vector3 velocity =
            moveInput * walkSpeed;

        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

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

        if (jumpPressed && isGrounded)
        {
            rb.AddForce(
                Vector3.up * jumpForce,
                ForceMode.Impulse
            );

            anim.SetTrigger("Jump");

            isGrounded = false;
        }

        jumpPressed = false;
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

        anim.SetBool("isSwimming", isInWater);
        anim.SetBool("isGrounded", isGrounded);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;

            rb.linearDamping = waterDrag;

            if (staminaUI != null)
                staminaUI.SetVisible(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;

            rb.linearDamping = 0f;

            if (staminaUI != null)
                staminaUI.SetVisible(false);

            anim.SetBool("isSwimming", false);
            anim.SetFloat("Speed", 0f);
        }
    }

    public void OnFormActivated() => enabled = true;
    public void OnFormDeactivated() => enabled = false;
}