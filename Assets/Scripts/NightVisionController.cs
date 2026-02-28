using UnityEngine;

[ExecuteInEditMode]
public class NightVisionController : MonoBehaviour
{
    public Material nightVisionMaterial;

    private bool active;

    public void EnableNightVision(bool value)
    {
        active = value;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (active && nightVisionMaterial != null)
            Graphics.Blit(src, dest, nightVisionMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
