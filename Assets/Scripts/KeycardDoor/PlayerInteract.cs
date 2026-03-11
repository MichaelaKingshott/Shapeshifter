using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public float interactDistance = 3f;

    PlayerInventory playerInventory;

    public TMP_Text popupText;

    public string keycardMessage = "Press E to pick up Keycard";
    public string generatorMessage = "Press E to start Generator";

    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

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

            // GENERATOR
            GeneratorLever generator = hit.collider.GetComponent<GeneratorLever>();

            if (generator != null)
            {
                popupText.text = generatorMessage;
                popupText.gameObject.SetActive(true);
                lookingAtItem = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    generator.Activate();
                }
            }
        }

        if (!lookingAtItem)
        {
            popupText.gameObject.SetActive(false);
        }
    }
}