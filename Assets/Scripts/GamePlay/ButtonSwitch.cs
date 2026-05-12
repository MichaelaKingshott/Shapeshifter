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
