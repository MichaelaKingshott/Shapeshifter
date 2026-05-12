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
    public Material greenMaterial;
    public Material redMaterial;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;

    private bool state = false;
    private bool isLookingAtButton = false;

    private InteractableHighlight highlight;

    void Start()
    {
        fanA.SetFanState(true);
        fanB.SetFanState(false);

        if (interactPopup != null)
            interactPopup.SetActive(false);

        highlight = GetComponentInChildren<InteractableHighlight>();

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
            FanSwitchButton button =
                hit.collider.GetComponentInParent<FanSwitchButton>();

            if (button == this)
            {
                hitThisButton = true;

                if (highlight != null)
                    highlight.ShowHighlight();
            }
        }

        if (!hitThisButton)
        {
            if (highlight != null)
                highlight.HideHighlight();
        }

        isLookingAtButton = hitThisButton;

        if (interactPopup != null)
        {
            interactPopup.SetActive(hitThisButton);
        }
    }

    public void Press()
    {
        state = !state;

        fanA.SetFanState(!state);
        fanB.SetFanState(state);

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