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

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Works even if this is a prefab
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            StartGrapple();

        if (Input.GetMouseButtonUp(0))
            StopGrapple();

        // Clamp rope length so it NEVER extends
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

    void StartGrapple()
    {
        if (joint != null) return; // prevent stacking

        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxDistance, grappleLayer))
        {
            grapplePoint = hit.point;

            joint = gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(transform.position, grapplePoint);

            // Lock rope length tighter
            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = 0f;

            // Stronger, less stretchy feel
            joint.spring = 80f;
            joint.damper = 15f;
            joint.massScale = 4.5f;

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