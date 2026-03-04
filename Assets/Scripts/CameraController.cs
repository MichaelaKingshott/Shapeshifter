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

    float yaw = 0f;
    float pitch = 10f;

    bool isFirstPerson = false;
    Vector3 currentOffset;
    public Camera cam;

    void Start()
    {
        currentOffset = thirdPersonOffset;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Mouse input
        yaw += Input.GetAxis("Mouse X") * rotateSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotateSpeed;
        pitch = Mathf.Clamp(pitch, minY, maxY);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Smoothly transition between offsets
        Vector3 desiredOffset = isFirstPerson ? firstPersonOffset : thirdPersonOffset;
        currentOffset = Vector3.Lerp(currentOffset, desiredOffset, smoothSpeed * Time.deltaTime);

        transform.position = target.position + rotation * currentOffset;

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
        }
        else
        {
            cam.cullingMask |= (1 << LayerMask.NameToLayer("PlayerMesh"));
        }
    }
}
