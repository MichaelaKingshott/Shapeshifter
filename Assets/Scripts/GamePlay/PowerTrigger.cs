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

                DestroyBlackoutEnemies(); // destroy selected enemies

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Snake form not unlocked yet.");
            }
        }
    }

    private void DestroyBlackoutEnemies()
    {
        BlackoutDestroyable[] enemies =
            FindObjectsByType<BlackoutDestroyable>(FindObjectsSortMode.None);

        foreach (BlackoutDestroyable enemy in enemies)
        {
            if (enemy.destroyOnBlackout)
            {
                Destroy(enemy.gameObject);
            }
        }
    }
}
