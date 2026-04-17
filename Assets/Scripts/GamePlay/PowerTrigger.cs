using UnityEngine;

public class PowerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShapeshifterController player = other.GetComponentInParent<ShapeshifterController>();

            if (player != null && player.IsFormUnlocked(AnimalForm.Snake))
            {
                PowerSystem.instance.TriggerBlackout();
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Snake form not unlocked yet.");
            }
        }
    }
}
