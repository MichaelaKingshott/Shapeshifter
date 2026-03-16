using UnityEngine;

[ExecuteInEditMode]
public class NightVisionController : MonoBehaviour
{
    public Material nightVisionMaterial;

    public bool Active { get; private set; }

    public void EnableNightVision(bool value)
    {
        Active = value;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Active && nightVisionMaterial != null)
            Graphics.Blit(src, dest, nightVisionMaterial);
        else
            Graphics.Blit(src, dest);
    }
}
