using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    [Header("Third Person")]
    public Vector3 thirdPersonOffset = new Vector3(0, 3, -5);

    [Header("First Person")]
    public Vector3 firstPersonOffset = new Vector3(0, 1.6f, 0);

    [Header("Look Settings")]
    public float lookHeight = 1.5f;

    [Header("Settings")]
    public float rotateSpeed = 5f;
    public float minY = -30f;
    public float maxY = 60f;
    public float smoothSpeed = 10f;

    [Header("Collision")]
    public float cameraRadius = 0.5f;
    public LayerMask collisionLayers;

    [Header("UI")]
    public GameObject crosshair;

    public Camera cam;

    private float yaw = 0f;
    private float pitch = 10f;

    private bool isFirstPerson = false;
    private Vector3 currentOffset;

    private bool controlsLocked = false;

    void Start()
    {
        currentOffset = thirdPersonOffset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (crosshair != null)
            crosshair.SetActive(false);

        // 🔥 Ensure correct startup rendering state
        ApplyCullingState();
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

        Vector3 direction = desiredPosition - target.position;

        // Check for collision with other objects (e.g., walls, terrain)
        if (Physics.SphereCast(
            target.position,
            cameraRadius,
            direction.normalized,
            out RaycastHit hit,
            direction.magnitude,
            collisionLayers))
        {
            if (direction.magnitude > hit.distance)
            {
                desiredPosition = target.position + direction.normalized * (hit.distance - cameraRadius);
            }
        }

        RaycastHit floorHit;

        // Cast DOWN from ABOVE the target (guaranteed to hit floor)
        Vector3 rayOrigin = target.position + Vector3.up * 2f;

        if (Physics.Raycast(rayOrigin, Vector3.down, out floorHit, 10f, collisionLayers))
        {
            float minHeight = floorHit.point.y + cameraRadius;

            if (desiredPosition.y < minHeight)
            {
                desiredPosition.y = minHeight;
            }
        }

        transform.position = desiredPosition;

        if (isFirstPerson)
        {
            transform.rotation = rotation;

            Quaternion targetRotation = Quaternion.Euler(0f, yaw, 0f);
            target.rotation = Quaternion.Slerp(
                target.rotation,
                targetRotation,
                15f * Time.deltaTime
            );
        }
        else
        {
            transform.LookAt(target.position + Vector3.up * lookHeight);
        }
    }

    public void SetFirstPerson(bool value)
    {
        isFirstPerson = value;

        ApplyCullingState();

        if (crosshair != null)
            crosshair.SetActive(value);
    }

    private void ApplyCullingState()
    {
        if (cam == null) return;

        int playerLayer = LayerMask.NameToLayer("PlayerMesh");

        if (playerLayer == -1)
        {
            Debug.LogError("Layer 'PlayerMesh' does not exist!");
            return;
        }

        if (isFirstPerson)
            cam.cullingMask &= ~(1 << playerLayer);
        else
            cam.cullingMask |= (1 << playerLayer);
    }

    public void LockCameraControls(bool locked)
    {
        controlsLocked = locked;
    }

    public void ApplyAnimalCameraSettings(AnimalCameraSettings settings)
    {
        if (settings == null) return;

        thirdPersonOffset = settings.thirdPersonOffset;
        firstPersonOffset = settings.firstPersonOffset;
        lookHeight = settings.lookHeight;

        currentOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
    }
}