using UnityEngine;

public class PlayerGrapple : MonoBehaviour
{
    [Header("References")]
    public Camera cam;
    public Transform gunTip;
    public LineRenderer lr;
    public LayerMask grappleLayer;

    [Header("Grapple Settings")]
    public float maxDistance = 40f;
    public float maxDistanceMultiplier = 0.8f;
    public float minDistanceMultiplier = 0.25f;

    [Header("Spring Settings")]
    public float spring = 4.5f;
    public float damper = 7f;
    public float massScale = 4.5f;

    private Vector3 grapplePoint;
    private SpringJoint joint;
    private Rigidbody rb;

    // Highlight system
    private GameObject currentGrapple;
    private Outline currentOutline;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        CheckForGrappleTarget();

        if (Input.GetMouseButtonDown(0))
            StartGrapple();

        if (Input.GetMouseButtonUp(0))
            StopGrapple();

        // Prevent rope extending
        if (joint != null)
        {
            float distance = Vector3.Distance(transform.position, grapplePoint);
            joint.maxDistance = Mathf.Min(distance, joint.maxDistance);
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

    // =========================
    // 🎯 TARGET DETECTION
    // =========================
    void CheckForGrappleTarget()
    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, grappleLayer))
        {
            if (hit.collider.CompareTag("Grapple"))
            {
                GameObject newTarget = hit.collider.gameObject;

                if (currentGrapple != newTarget)
                {
                    ClearHighlight();

                    currentGrapple = newTarget;
                    currentOutline = currentGrapple.GetComponent<Outline>();

                    if (currentOutline != null)
                        currentOutline.enabled = true;
                }

                return;
            }
        }

        ClearHighlight();
    }

    void ClearHighlight()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
        }

        currentGrapple = null;
        currentOutline = null;
    }

    // =========================
    // 🪝 GRAPPLE LOGIC
    // =========================
    void StartGrapple()
    {
        if (joint != null) return;

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, grappleLayer))
        {
            if (!hit.collider.CompareTag("Grapple")) return;

            grapplePoint = hit.point;

            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * maxDistanceMultiplier;
            joint.minDistance = 0f;

            joint.spring = 80f;
            joint.damper = 15f;
            joint.massScale = massScale;

            lr.positionCount = 2;
        }
    }

    void StopGrapple()
    {
        lr.positionCount = 0;

        if (joint != null)
            Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }
}