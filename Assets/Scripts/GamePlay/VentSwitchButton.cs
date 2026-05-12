using UnityEngine;

public class VentSwitchButton : MonoBehaviour
{
    public VentBlocker vent;

    public GameObject interactPopup;

    public Renderer buttonRenderer;

    public Material greenMaterial; // vent open
    public Material redMaterial;   // vent closed

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    [Header("Raycast")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;

    private bool isLookingAtButton = false;
    private bool state = false; // false = closed, true = open

    private InteractableHighlight highlight;

    void Start()
    {
        if (interactPopup != null)
            interactPopup.SetActive(false);

        highlight = GetComponentInChildren<InteractableHighlight>();

        UpdateButtonMaterial();
        vent.SetOpen(false);
    }

    void Update()
    {
        CheckRaycast();

        if (isLookingAtButton && Input.GetKeyDown(KeyCode.E))
        {
            Press();
        }
    }

    void CheckRaycast()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        bool hitThisButton = false;

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            VentSwitchButton button =
                hit.collider.GetComponentInParent<VentSwitchButton>();

            if (button == this)
            {
                hitThisButton = true;

                if (highlight != null)
                    highlight.ShowHighlight();

                if (interactPopup != null)
                    interactPopup.SetActive(true);
            }
        }

        if (!hitThisButton)
        {
            if (highlight != null)
                highlight.HideHighlight();

            if (interactPopup != null)
                interactPopup.SetActive(false);
        }

        isLookingAtButton = hitThisButton;
    }

    public void Press()
    {
        // Play click sound
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound, 1f);

        state = !state;

        vent.SetOpen(state);
        UpdateButtonMaterial();
    }

    void UpdateButtonMaterial()
    {
        if (buttonRenderer == null) return;

        buttonRenderer.material = state ? greenMaterial : redMaterial;
    }
}
