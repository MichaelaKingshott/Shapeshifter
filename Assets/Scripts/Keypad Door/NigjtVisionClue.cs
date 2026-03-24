using UnityEngine;
using UnityEngine.UI; // IMPORTANT

public class NightVisionClue : MonoBehaviour
{
    private NightVisionController nightVision;
    private ShapeshifterController shapeshifter;

    private Renderer[] renderers;
    private Graphic[] graphics;

    void Start()
    {
        nightVision = Camera.main.GetComponent<NightVisionController>();
        shapeshifter = FindFirstObjectByType<ShapeshifterController>();

        // Get ALL renderers (for 3D objects)
        renderers = GetComponentsInChildren<Renderer>(true);

        // Get ALL UI graphics (for UI objects)
        graphics = GetComponentsInChildren<Graphic>(true);
    }

    void Update()
    {
        if (nightVision == null || shapeshifter == null) return;

        bool shouldShow = nightVision.Active && !shapeshifter.IsInvisible();

        // Handle 3D objects
        foreach (var r in renderers)
            r.enabled = shouldShow;

        // Handle UI elements
        foreach (var g in graphics)
            g.enabled = shouldShow;
    }
}