using UnityEngine;

public class CameraWaterDetector : MonoBehaviour
{
    private UnderwaterEffectController underwaterEffect;

    void Start()
    {
        underwaterEffect = GetComponent<UnderwaterEffectController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            underwaterEffect.SetUnderwater(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            underwaterEffect.SetUnderwater(false);
        }
    }
}