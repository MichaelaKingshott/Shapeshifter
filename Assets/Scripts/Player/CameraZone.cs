using UnityEngine;

public class CameraZone : MonoBehaviour
{
    public CameraController cameraController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            cameraController.SetFirstPerson(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            cameraController.SetFirstPerson(false);
    }
}
