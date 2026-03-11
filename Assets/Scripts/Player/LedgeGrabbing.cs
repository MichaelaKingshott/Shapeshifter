using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    public Transform ledgeDetection;
    public Transform ledgeClearance;

    private Rigidbody rb;
    private MonkeyMovement movement;

    [Header("Detection")]
    public float forwardCheckDistance = 0.9f;
    public float downwardCheckDistance = 1.4f;
    public float sphereRadius = 0.25f;
    public float clearanceRadius = 0.35f;
    public LayerMask ledgeLayer;

    [Header("Jump Settings")]
    public float jumpUpForce = 7f;
    public float jumpForwardForce = 4f;

    [Header("Grab Cooldown")]
    public float grabCooldown = 0.25f;

    private bool isHanging = false;
    private float lastGrabTime;

    private Vector3 hangPosition;
    private Vector3 wallNormal;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<MonkeyMovement>();

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
        // Only grab while falling
        if (rb.linearVelocity.y > 0)
            return;

        RaycastHit forwardHit;

        if (Physics.SphereCast(
            ledgeDetection.position,
            sphereRadius,
            transform.forward,
            out forwardHit,
            forwardCheckDistance,
            ledgeLayer))
        {
            RaycastHit topHit;

            Vector3 downRayStart = forwardHit.point + Vector3.up * 0.6f;

            if (Physics.Raycast(
                downRayStart,
                Vector3.down,
                out topHit,
                downwardCheckDistance,
                ledgeLayer))
            {
                // Check space above ledge
                if (!Physics.CheckSphere(ledgeClearance.position, clearanceRadius, ledgeLayer))
                {
                    float ledgeHeight = topHit.point.y - transform.position.y;

                    if (ledgeHeight < 0.4f)
                        return;

                    GrabLedge(topHit, forwardHit.normal);
                }
            }
        }
    }

    void GrabLedge(RaycastHit hit, Vector3 normal)
    {
        isHanging = true;
        lastGrabTime = Time.time;

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;
        rb.isKinematic = true;

        if (movement != null)
            movement.enabled = false;

        wallNormal = normal;

        hangPosition = hit.point - transform.forward * 0.45f;
        hangPosition.y -= 0.6f;

        transform.position = hangPosition;

        Vector3 wallForward = -normal;
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

        rb.isKinematic = false;
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

        rb.isKinematic = false;
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

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(ledgeDetection.position + transform.forward * forwardCheckDistance, sphereRadius);
        }

        if (ledgeClearance != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ledgeClearance.position, clearanceRadius);
        }
    }
}