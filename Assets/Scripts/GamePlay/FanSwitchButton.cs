using UnityEngine;

public class FanSwitchButton : MonoBehaviour, IPressable
{
    public FanSpin fanA;
    public FanSpin fanB;

    public GameObject interactPopup;

    private bool state = false; // false = A on, true = B on

    public Renderer buttonRenderer;

    public Material greenMaterial; // Fan A active
    public Material redMaterial;   // Fan B active

    // 👇 Raycast settings
    public float interactDistance = 3f;
    public LayerMask interactLayer;
    public Camera playerCamera;

    private bool isLookingAtButton = false;

    void Start()
    {
        fanA.SetFanState(true);
        fanB.SetFanState(false);

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

        if (Physics.Raycast(ray, out hit, interactDistance, interactLayer))
        {
            // Check if THIS object was hit
            if (hit.collider.gameObject == gameObject)
            {
                isLookingAtButton = true;
                interactPopup.SetActive(true);
                return;
            }
        }

        // If we didn't hit this button
        isLookingAtButton = false;
        interactPopup.SetActive(false);
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
        if (buttonRenderer == null) return;

        if (state)
            buttonRenderer.material = redMaterial;
        else
            buttonRenderer.material = greenMaterial;
    }
}
