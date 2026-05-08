using UnityEngine;

public class FanSwitchButton : MonoBehaviour, IPressable
{
    [Header("Fans")]
    public FanSpin fanA;
    public FanSpin fanB;

    [Header("UI")]
    public GameObject interactPopup;

    [Header("Button Visuals")]
    public Renderer buttonRenderer;
    public Material greenMaterial; // Fan A active
    public Material redMaterial;   // Fan B active

    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;

    private bool state = false; // false = A on, true = B on
    private bool isLookingAtButton = false;

    void Start()
    {
        // Initial fan states
        fanA.SetFanState(true);
        fanB.SetFanState(false);

        // Hide popup initially
        if (interactPopup != null)
            interactPopup.SetActive(false);

        UpdateButtonMaterial();
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
            // Look for FanSwitchButton on hit object or its parents
            FanSwitchButton button = hit.collider.GetComponentInParent<FanSwitchButton>();

            if (button == this)
            {
                hitThisButton = true;
            }
        }

        // Update interaction state
        isLookingAtButton = hitThisButton;

        // Show/hide popup
        if (interactPopup != null)
        {
            interactPopup.SetActive(hitThisButton);
        }
    }

    public void Press()
    {
        // Toggle state
        state = !state;

        // Switch fans
        fanA.SetFanState(!state);
        fanB.SetFanState(state);

        // Update button color
        UpdateButtonMaterial();
    }

    void UpdateButtonMaterial()
    {
        if (buttonRenderer == null)
            return;

        if (state)
            buttonRenderer.material = redMaterial;
        else
            buttonRenderer.material = greenMaterial;
    }
}