using UnityEngine;

public class AnimalCameraSettings : MonoBehaviour
{
    [Header("Camera Offset")]
    public Vector3 thirdPersonOffset = new Vector3(0, 3, -5);

    [Header("Look At Height")]
    public float lookHeight = 1.5f;
}
