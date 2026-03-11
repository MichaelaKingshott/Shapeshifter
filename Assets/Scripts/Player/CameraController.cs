using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [Header("Third Person")]
    public Vector3 thirdPersonOffset = new Vector3(0, 3, -5);

    [Header("First Person")]
    public Vector3 firstPersonOffset = new Vector3(0, 1.6f, 0);

    [Header("Settings")]
    public float rotateSpeed = 5f;
    public float minY = -30f;
    public float maxY = 60f;
    public float smoothSpeed = 10f;

    [Header("Collision")]
    public float cameraRadius = 0.2f;
    public LayerMask collisionLayers;

    [Header("UI")]
    public GameObject crosshair;

    float yaw = 0f;
    float pitch = 10f;

    bool isFirstPerson = false;
    Vector3 currentOffset;

    public Camera cam;

    bool controlsLocked = false;

    void Start()
    {
        currentOffset = thirdPersonOffset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshair != null)
        {
            crosshair.SetActive(false);
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        if (!controlsLocked)
        {
            yaw += Input.GetAxis("Mouse X") * rotateSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotateSpeed;
            pitch = Mathf.Clamp(pitch, minY, maxY);
        }

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 desiredOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
        currentOffset = Vector3.Lerp(currentOffset, desiredOffset, smoothSpeed * Time.deltaTime);

        Vector3 desiredPosition = target.position + rotation * currentOffset;

        RaycastHit hit;
        Vector3 direction = desiredPosition - target.position;

        if (Physics.SphereCast(target.position, cameraRadius, direction.normalized, out hit, direction.magnitude, collisionLayers))
        {
            desiredPosition = hit.point - direction.normalized * cameraRadius;
        }

        transform.position = desiredPosition;

        if (isFirstPerson)
        {
            transform.rotation = rotation;
        }
        else
        {
            transform.LookAt(target.position + Vector3.up * 1.5f);
        }
    }

    public void SetFirstPerson(bool value)
    {
        isFirstPerson = value;

        if (value)
        {
            cam.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerMesh"));

            if (crosshair != null)
                crosshair.SetActive(true);
        }
        else
        {
            cam.cullingMask |= (1 << LayerMask.NameToLayer("PlayerMesh"));

            if (crosshair != null)
                crosshair.SetActive(false);
        }
    }

    public void LockCameraControls(bool locked)
    {
        controlsLocked = locked;
    }
}