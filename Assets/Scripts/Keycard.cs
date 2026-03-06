using UnityEngine;

public class Keycard : MonoBehaviour
{
    public KeycardType keycardType;

    public void Pickup(PlayerInventory player)
    {
        player.AddKeycard(keycardType);
        Destroy(gameObject);
    }
}
