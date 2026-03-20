using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public FanSpin targetObject;
    public bool toggle = true;

    public bool startActivated = true;
    private bool activated;

    public GameObject interactPopup;
    private bool playerInRange = false;

    public Renderer buttonRenderer;

    // 👇 NEW: assign your materials in Inspector
    public Material activeMaterial;   // Green
    public Material inactiveMaterial; // Red

    void Start()
    {
        interactPopup.SetActive(false);

        activated = startActivated;
        targetObject.SetFanState(activated);

        UpdateButtonMaterial();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        if (toggle)
        {
            activated = !activated;
            targetObject.SetFanState(activated);
        }
        else
        {
            activated = true;
            targetObject.SetFanState(true);
        }

        UpdateButtonMaterial();
    }

    void UpdateButtonMaterial()
    {
        if (buttonRenderer == null) return;

        if (activated)
        {
            buttonRenderer.material = activeMaterial;
        }
        else
        {
            buttonRenderer.material = inactiveMaterial;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPopup.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPopup.SetActive(false);
        }
    }
}