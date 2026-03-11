using UnityEngine;

public class Keycard : MonoBehaviour, IGrabbable
{
    public KeycardType keycardType;

    public void Pickup(PlayerInventory player)
    {
        player.AddKeycard(keycardType);
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
