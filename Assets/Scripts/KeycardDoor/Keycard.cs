using UnityEngine;

public class Keycard : MonoBehaviour, IGrabbable
{
    public KeycardType keycardType;

    [Header("Audio")]
    public AudioClip pickupSound;

    public void Pickup(PlayerInventory player)
    {
        player.AddKeycard(keycardType);

        // PLAY PICKUP SOUND
        if (pickupSound != null)
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);

        Destroy(gameObject);
    }

    public void OnGrab(Transform tongue)
    {
        Debug.Log("Keycard grabbed by tongue");

        PlayerInventory player = FindFirstObjectByType<PlayerInventory>();

        if (player != null)
        {
            Pickup(player);
        }
        else
        {
            Debug.LogWarning("No PlayerInventory found in scene!");
        }
    }
}
