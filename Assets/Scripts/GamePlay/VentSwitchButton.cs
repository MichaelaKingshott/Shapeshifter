using UnityEngine;

public class VentSwitchButton : MonoBehaviour
{
    public VentBlocker vent;

    public GameObject interactPopup;

    public Renderer buttonRenderer;

    public Material greenMaterial; // vent open
    public Material redMaterial;   // vent closed

    [Header("Raycast")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;

    private bool isLookingAtButton = false;
    private bool state = false; // false = closed, true = open

    void Start()
    {
        if (interactPopup != null)
            interactPopup.SetActive(false);

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

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            if (hit.collider.gameObject == gameObject)
            {
                isLookingAtButton = true;

                if (interactPopup != null)
                    interactPopup.SetActive(true);

                return;
            }
        }

        isLookingAtButton = false;

        if (interactPopup != null)
            interactPopup.SetActive(false);
    }

    public void Press()
    {
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
