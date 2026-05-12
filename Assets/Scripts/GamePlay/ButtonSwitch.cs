using UnityEngine;

public class ButtonSwitch : MonoBehaviour, IPressable
{
    public MovingObject targetObject;
    public bool toggle = true;

    private bool activated = false;

    [Header("Visuals")]
    public Renderer buttonRenderer;
    public Material onMaterial;   // Green
    public Material offMaterial;  // Red

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    private void Start()
    {
        UpdateVisual();
    }

    void UpdateVisual()
    {
        if (buttonRenderer == null) return;

        buttonRenderer.material = activated ? onMaterial : offMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("InvisiblePlayer"))
        {
            Press();
        }
    }

    public void Press()
    {
        if (targetObject == null) return;

        // Play click sound
        if (audioSource != null && buttonClickSound != null)
            audioSource.PlayOneShot(buttonClickSound, 1f);

        if (toggle)
        {
            activated = !activated;
        }
        else
        {
            activated = true;
        }

        targetObject.SetActiveState(activated);

        UpdateVisual();

        Debug.Log("Button pressed!");
    }
}
