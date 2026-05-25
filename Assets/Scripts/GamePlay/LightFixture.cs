using UnityEngine;

public class LightFixture : MonoBehaviour
{
    [Header("Extra Effects")]
    public GameObject[] effects;

    private Light[] childLights;

    private void Awake()
    {
        // Automatically find ALL lights in children
        childLights = GetComponentsInChildren<Light>(true);
    }

    public void SetState(bool state)
    {
        // Toggle every child light
        foreach (Light l in childLights)
        {
            if (l != null)
                l.enabled = state;
        }

        // Toggle effects
        foreach (GameObject fx in effects)
        {
            if (fx != null)
                fx.SetActive(state);
        }
    }
}