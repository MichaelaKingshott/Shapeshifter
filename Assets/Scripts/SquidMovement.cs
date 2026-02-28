using UnityEngine;

public class SquidMovement : MonoBehaviour, IAnimalAbility
{
    [Header("General")]
    public float turnSpeed = 6f;
    private Rigidbody rb;
    private bool isInWater = false;
    private bool isGrounded = false;

    [Header("Swimming")]
    public float swimSpeed = 10f;          // horizontal speed
    public float verticalSwimSpeed = 6f;   // up/down speed
    public float hoverStrength = 2f;       // how strongly the squid hovers at target depth
    public float waterDrag = 2f;
    public float velocitySmooth = 0.2f;    // smooth factor for velocity changes
    private float targetDepth;             // y position to hover at

    [Header("Walking")]
    public float walkSpeed = 3f;
    public float jumpForce = 5f;

    [Header("Animation")]
    public float moveThreshold = 0.1f;     // minimum speed to trigger walking/swimming
    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isInWater) HandleSwimming();
        else HandleWalking();
    }

    void HandleSwimming()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        float yInput = 0f;
        if (Input.GetKey(KeyCode.Space)) yInput = 1f;
        if (Input.GetKey(KeyCode.LeftShift)) yInput = -1f;

        // Camera-relative horizontal movement
        Vector3 camForward = Camera.main.transform.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right; camRight.y = 0; camRight.Normalize();
        Vector3 moveXZ = camForward * v + camRight * h;

        // Determine vertical velocity
        float verticalVel;
        if (yInput != 0f)
        {
            verticalVel = yInput * verticalSwimSpeed;
            targetDepth = transform.position.y; // reset hover target
        }
        else
        {
            float depthDiff = targetDepth - transform.position.y;
            verticalVel = depthDiff * hoverStrength;
        }

        // Apply smoothed velocity
        Vector3 targetVelocity = new Vector3(moveXZ.x * swimSpeed, verticalVel, moveXZ.z * swimSpeed);
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, velocitySmooth);

        // Smooth rotation toward horizontal movement
        Vector3 flatMove = new Vector3(moveXZ.x, 0, moveXZ.z);
        if (flatMove != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(flatMove, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Update Animator
        float horizontalSpeed = flatMove.magnitude;
        anim.SetFloat("Speed", horizontalSpeed < moveThreshold ? 0f : horizontalSpeed);
        anim.SetBool("IsSwimming", true);
        anim.SetBool("IsJumping", false);
    }

    void HandleWalking()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward; camForward.y = 0; camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right; camRight.y = 0; camRight.Normalize();

        Vector3 move = (camForward * v + camRight * h).normalized;
        Vector3 velocity = move * walkSpeed;
        velocity.y = rb.linearVelocity.y;
        rb.linearVelocity = velocity;

        // Rotate toward movement
        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // Update Animator
        Vector3 horizontalVel = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = horizontalVel.magnitude;
        anim.SetFloat("Speed", speed < moveThreshold ? 0f : speed);
        anim.SetBool("IsJumping", !isGrounded);
        anim.SetBool("IsSwimming", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.useGravity = false;
            rb.linearDamping = waterDrag;

            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            targetDepth = transform.position.y;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.useGravity = true;
            rb.linearDamping = 0;
            
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            anim.SetBool("IsSwimming", false);
            anim.SetFloat("Speed", 0f);
        }
    }

    public void OnFormActivated() => this.enabled = true;
    public void OnFormDeactivated() => this.enabled = false;
}
