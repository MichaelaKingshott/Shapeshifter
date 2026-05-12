using UnityEngine;

public class FanButton : MonoBehaviour
{
    public FanSpin targetFan;

    [Header("Visuals")]
    public Renderer buttonRenderer;
    public Material onMaterial;   // Green
    public Material offMaterial;  // Red

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    private bool playerInRange;

    private InteractableHighlight highlight;

    void Start()
    {
        UpdateVisual();

        highlight = GetComponent<InteractableHighlight>();
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

        // Play click sound
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound, 1f);

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

            if (highlight != null)
                highlight.ShowHighlight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            InteractionPromptUI.Instance.HidePrompt();

            if (highlight != null)
                highlight.HideHighlight();
        }
    }   
}