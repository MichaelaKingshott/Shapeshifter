using UnityEngine;
using TMPro;

public class PlayerInteract : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 3f;
    public float sphereRadius = 0.15f;

    PlayerInventory playerInventory;

    public TMP_Text popupText;
    public string keycardMessage = "Press E to pick up Keycard";

    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        bool lookingAtItem = false;

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactDistance))
        {
            Keycard keycard = hit.collider.GetComponentInParent<Keycard>();

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