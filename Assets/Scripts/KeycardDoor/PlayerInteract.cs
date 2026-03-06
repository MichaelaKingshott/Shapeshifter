using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public PlayerInventory playerInventory;

    public TMP_Text popupText;   // assign in inspector
    public string keycardMessage = "Press E to pick up Keycard";

    void Update()
    {
        RaycastHit hit;
        bool lookingAtItem = false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            // KEYCARD
            Keycard keycard = hit.collider.GetComponent<Keycard>();

            if (keycard != null)
            {
                popupText.text = keycardMessage;
                popupText.gameObject.SetActive(true);
                lookingAtItem = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    keycard.Pickup(playerInventory);
                }
            }
        }

        if (!lookingAtItem)
        {
            popupText.gameObject.SetActive(false);
        }
    }
}