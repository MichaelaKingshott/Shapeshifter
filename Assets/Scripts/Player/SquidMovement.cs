using UnityEngine;

public class SquidMovement : MonoBehaviour, IAnimalAbility
{
    [Header("References")]
    public SquidStaminaUI staminaUI;

    [Header("General")]
    public float turnSpeed = 6f;

    private Rigidbody rb;
    private Animator anim;

    private bool isInWater = false;
    private bool isGrounded = false;

    [Header("Swimming")]
    public float swimSpeed = 6f;
    public float swimBurstForce = 6f;
    public float waterDrag = 3f;

    [Header("Swim Spam")]
    public float swimSpamDelay = 0.12f;
    private float lastSwimPress;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaDrain = 1f;
    public float staminaRegen = 2f;

    private float stamina;

    [Header("Walking")]
    public float walkSpeed = 3f;
    public float jumpForce = 5f;

    [Header("Animation")]
    public float moveThreshold = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        stamina = maxStamina;

        if (staminaUI == null)
            staminaUI = FindFirstObjectByType<SquidStaminaUI>();

        if (staminaUI != null)
            staminaUI.UpdateStamina(1f);
    }

    void Update()
    {
        if (isInWater)
            HandleSwimming();
        else
            HandleWalking();
    }

    void HandleSwimming()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = camForward * v + camRight * h;

        Vector3 velocity = rb.linearVelocity;

        if (stamina > 0)
        {
            velocity.x = move.x * swimSpeed;
            velocity.z = move.z * swimSpeed;
        }
        else
        {
            velocity.x = 0;
            velocity.z = 0;
        }

        rb.linearVelocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) &&
            stamina > 0 &&
            Time.time > lastSwimPress + swimSpamDelay)
        {
            rb.AddForce(Vector3.up * swimBurstForce, ForceMode.Impulse);

            stamina -= staminaDrain;
            lastSwimPress = Time.time;
        }

        if (move.magnitude < 0.1f && !Input.GetKey(KeyCode.Space))
        {
            stamina += staminaRegen * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        if (staminaUI != null)
            staminaUI.UpdateStamina(stamina / maxStamina);

        Vector3 flatMove = new Vector3(move.x, 0, move.z);

        if (flatMove != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatMove);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        float horizontalSpeed = flatMove.magnitude;

        anim.SetFloat("Speed", horizontalSpeed < moveThreshold ? 0 : horizontalSpeed);
        anim.SetBool("isSwimming", true);
        anim.SetBool("isGrounded", false);
    }

    void HandleWalking()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * v + camRight * h).normalized;

        Vector3 velocity = move * walkSpeed;
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.deltaTime
            );
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            anim.SetTrigger("Jump");
            isGrounded = false;
        }

        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = horizontalVel.magnitude;

        anim.SetFloat("Speed", speed < moveThreshold ? 0 : speed);
        anim.SetBool("isSwimming", false);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;

            if (staminaUI != null)
                staminaUI.SetVisible(true);

            rb.useGravity = true;
            rb.linearDamping = waterDrag;

            rb.constraints =
                RigidbodyConstraints.FreezeRotationX |
                RigidbodyConstraints.FreezeRotationZ;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;

            if (staminaUI != null)
                staminaUI.SetVisible(false);

            rb.useGravity = true;
            rb.linearDamping = 0;

            anim.SetBool("isSwimming", false);
            anim.SetFloat("Speed", 0);
        }
    }

    public void OnFormActivated() => enabled = true;
    public void OnFormDeactivated() => enabled = false;
}