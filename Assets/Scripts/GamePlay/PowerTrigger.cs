using UnityEngine;

public class PowerTrigger : MonoBehaviour
{
    [Header("Audio")]
    public AudioClip blackoutSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShapeshifterController player = other.GetComponentInParent<ShapeshifterController>();

            if (player != null && player.IsFormUnlocked(AnimalForm.Snake))
            {
                // PLAY BLACKOUT SOUND
                if (blackoutSound != null)
                    AudioSource.PlayClipAtPoint(blackoutSound, transform.position);

                PowerSystem.instance.TriggerBlackout();

                DestroyBlackoutEnemies();

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
