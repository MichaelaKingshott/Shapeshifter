using UnityEngine;

public class TongueController : MonoBehaviour
{
    public Transform tongueOrigin;
    public LineRenderer lr;
    public LayerMask tongueLayer;

    public float maxDistance = 20f;
    public float tongueDuration = 0.25f;

    private Vector3 hitPoint;
    private bool tongueActive;
    private float timer;

    void Start()
    {
        lr.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShootTongue();

        if (tongueActive)
        {
            timer -= Time.deltaTime;

            if (timer <= 0)
                StopTongue();
        }
    }

    void LateUpdate()
    {
        if (!tongueActive) return;

        lr.SetPosition(0, tongueOrigin.position);
        lr.SetPosition(1, hitPoint);
    }

    void ShootTongue()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;

        if (Physics.Raycast(tongueOrigin.position, direction, out hit, maxDistance, tongueLayer))
        {
            Debug.Log("Tongue hit: " + hit.collider.name);

            hitPoint = hit.point;

            // Press buttons
            IPressable pressable = hit.collider.GetComponentInParent<IPressable>();
            if (pressable != null)
            {
                pressable.Press();
            }

            // Grab objects
            IGrabbable grabbable = hit.collider.GetComponentInParent<IGrabbable>();
            if (grabbable != null)
            {
                grabbable.OnGrab(tongueOrigin);
            }
        }
        else
        {
            hitPoint = tongueOrigin.position + direction * maxDistance;
        }

        lr.positionCount = 2;

        tongueActive = true;
        timer = tongueDuration;
    }

    void StopTongue()
    {
        tongueActive = false;
        lr.positionCount = 0;
    }
}