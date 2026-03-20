using UnityEngine;

public class FanSwitchButton : MonoBehaviour
{
    public FanSpin fanA;
    public FanSpin fanB;

    public GameObject interactPopup;

    private bool state = false; // false = A on, true = B on
    private bool playerInRange = false;

    public Renderer buttonRenderer;

    // 👇 Assign in Inspector
    public Material greenMaterial; // Fan A active
    public Material redMaterial;   // Fan B active

    void Start()
    {
        fanA.SetFanState(true);
        fanB.SetFanState(false);

        interactPopup.SetActive(false);

        UpdateButtonMaterial(); // set correct starting color
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
        state = !state;

        fanA.SetFanState(!state);
        fanB.SetFanState(state);

        UpdateButtonMaterial();
    }

    void UpdateButtonMaterial()
    {
        if (buttonRenderer == null) return;

        // false = Fan A ON → green
        // true = Fan B ON → red
        if (state)
        {
            buttonRenderer.material = redMaterial;
        }
        else
        {
            buttonRenderer.material = greenMaterial;
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
