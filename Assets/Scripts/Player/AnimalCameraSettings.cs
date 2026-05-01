using UnityEngine;

public class AnimalCameraSettings : MonoBehaviour
{
    [Header("Third Person")]
    public Vector3 thirdPersonOffset = new Vector3(0, 3, -5);

    [Header("First Person")]
    public Vector3 firstPersonOffset = new Vector3(0, 1.6f, 0);

    [Header("Look At Height")]
    public float lookHeight = 1.5f;
}
