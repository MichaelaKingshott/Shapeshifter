using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    public Transform ledgeDetection;
    public Transform ledgeClearance;
    public Transform grabPoint;

    private Rigidbody rb;
    private MonkeyMovement movement;
    private Collider playerCollider;

    [Header("Detection")]
    public float forwardCheckDistance = 0.7f;
    public float downwardCheckDistance = 1.2f;
    public float clearanceRadius = 0.35f;
    public LayerMask ledgeLayer;

    [Header("Jump Settings")]
    public float jumpUpForce = 7f;
    public float jumpForwardForce = 4f;

    [Header("Grab Cooldown")]
    public float grabCooldown = 0.25f;

    private bool isHanging = false;
    private float lastGrabTime;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<MonkeyMovement>();
        playerCollider = GetComponent<Collider>();

        // Prevent physics flips
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void Update()
    {
        if (isHanging)
        {
            HandleHangInput();
            return;
        }

        if (Time.time > lastGrabTime + grabCooldown)
        {
            DetectLedge();
        }
    }

    void DetectLedge()
    {
        RaycastHit forwardHit;

        // Step 1: Detect wall in front
        if (Physics.Raycast(ledgeDetection.position, transform.forward, out forwardHit, forwardCheckDistance, ledgeLayer))
        {
            RaycastHit topHit;

            // Step 2: Detect the top of the ledge
            Vector3 downRayStart = forwardHit.point + Vector3.up * 0.6f;

            if (Physics.Raycast(downRayStart, Vector3.down, out topHit, downwardCheckDistance, ledgeLayer))
            {
                // Step 3: Check if space above is free
                if (!Physics.CheckSphere(ledgeClearance.position, clearanceRadius, ledgeLayer))
                {
                    GrabLedge(topHit);
                }
            }
        }
    }

    void GrabLedge(RaycastHit hit)
    {
        isHanging = true;
        lastGrabTime = Time.time;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;

        // Freeze movement while hanging
        rb.constraints = RigidbodyConstraints.FreezeAll;

        // Disable collider so physics can't push player away
        playerCollider.enabled = false;

        if (movement != null)
            movement.enabled = false;

        // Snap to grab point
        transform.position = grabPoint.position;

        // Face the ledge
        Vector3 wallForward = -hit.normal;
        wallForward.y = 0;
        transform.rotation = Quaternion.LookRotation(wallForward);
    }

    void HandleHangInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpFromLedge();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            DropFromLedge();
        }
    }

    void JumpFromLedge()
    {
        isHanging = false;
        lastGrabTime = Time.time;

        playerCollider.enabled = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;

        if (movement != null)
            movement.enabled = true;

        rb.linearVelocity = Vector3.zero;

        Vector3 jumpDir = Vector3.up * jumpUpForce + transform.forward * jumpForwardForce;

        rb.AddForce(jumpDir, ForceMode.Impulse);
    }

    void DropFromLedge()
    {
        isHanging = false;
        lastGrabTime = Time.time;

        playerCollider.enabled = true;

        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.useGravity = true;

        if (movement != null)
            movement.enabled = true;
    }

    void OnDrawGizmos()
    {
        if (ledgeDetection != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(ledgeDetection.position, transform.forward * forwardCheckDistance);
        }

        if (ledgeClearance != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ledgeClearance.position, clearanceRadius);
        }
    }
}