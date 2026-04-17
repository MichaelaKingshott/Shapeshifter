using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;

        ShapeshifterController player = other.GetComponentInParent<ShapeshifterController>();

        if (player != null)
        {
            activated = true;

            GameManager.Instance.checkpointPosition = player.transform.position;
            GameManager.Instance.hasCheckpoint = true;

            Debug.Log("Checkpoint reached!");
        }
    }
}