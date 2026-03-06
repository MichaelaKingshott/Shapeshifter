using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public PlayerInventory playerInventory;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // KEYCARD
                Keycard keycard = hit.collider.GetComponent<Keycard>();
                if (keycard != null)
                {
                    keycard.Pickup(playerInventory);
                }
            }
        }
    }
}
