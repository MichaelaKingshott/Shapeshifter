using UnityEngine;
using UnityEngine.UI;

public class SquidStaminaUI : MonoBehaviour
{
    public Slider staminaBar;

    void Awake()
    {
        if (staminaBar != null)
        {
            staminaBar.minValue = 0f;
            staminaBar.maxValue = 1f;
            staminaBar.value = 1f;
            staminaBar.gameObject.SetActive(false);
        }
    }

    public void SetVisible(bool visible)
    {
        if (staminaBar != null)
            staminaBar.gameObject.SetActive(visible);
    }

    public void UpdateStamina(float normalizedValue)
    {
        if (staminaBar != null)
            staminaBar.value = normalizedValue;
    }
}