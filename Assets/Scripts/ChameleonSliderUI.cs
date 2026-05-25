using UnityEngine;
using UnityEngine.UI;

public class ChameleonSliderUI : MonoBehaviour
{
    public Slider slider;

    [SerializeField] private GameObject visuals;

    void Awake()
    {
        visuals.SetActive(false);
    }

    public void Show()
    {
        visuals.SetActive(true);
    }

    public void Hide()
    {
        visuals.SetActive(false);
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
}