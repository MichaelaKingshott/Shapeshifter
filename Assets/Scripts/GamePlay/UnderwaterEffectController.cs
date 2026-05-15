using UnityEngine;

[ExecuteInEditMode]
public class UnderwaterEffectController : MonoBehaviour
{
    public Material underwaterMaterial;

    public bool Active { get; private set; }

    public void SetUnderwater(bool value)
    {
        Active = value;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Active && underwaterMaterial != null)
            Graphics.Blit(src, dest, underwaterMaterial);
        else
            Graphics.Blit(src, dest);
    }
}