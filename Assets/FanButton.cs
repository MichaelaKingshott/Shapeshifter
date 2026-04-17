using UnityEngine;

public class FanButton : MonoBehaviour
{
    public FanSpin targetFan;

    [Header("Visuals")]
    public Renderer buttonRenderer;
    public Material onMaterial;   // Green
    public Material offMaterial;  // Red

    private bool playerInRange;

    void Start()
    {
        UpdateVisual();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleFan();
        }
    }

    void ToggleFan()
    {
        if (targetFan == null) return;

        bool newState = !targetFan.isOn;
        targetFan.SetFanState(newState);

        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (buttonRenderer == null || targetFan == null) return;

        buttonRenderer.material = targetFan.isOn ? onMaterial : offMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            InteractionPromptUI.Instance.ShowPrompt("Press E to toggle fan");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            InteractionPromptUI.Instance.HidePrompt();
        }
    }
}