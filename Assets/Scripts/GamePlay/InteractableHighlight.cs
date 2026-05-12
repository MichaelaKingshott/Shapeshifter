using UnityEngine;

public class InteractableHighlight : MonoBehaviour
{
    private Outline outline;

    void Awake()
    {
        outline = GetComponent<Outline>();

        if (outline != null)
            outline.enabled = false;
    }

    public void ShowHighlight()
    {
        if (outline != null)
            outline.enabled = true;
    }

    public void HideHighlight()
    {
        if (outline != null)
            outline.enabled = false;
    }
}