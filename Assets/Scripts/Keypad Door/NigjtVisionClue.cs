using UnityEngine;

public class NightVisionClue : MonoBehaviour
{
    private NightVisionController nightVision;

    void Start()
    {
        nightVision = Camera.main.GetComponent<NightVisionController>();
    }

    void Update()
    {
        if (nightVision == null) return;

        gameObject.SetActive(nightVision.Active);
    }
}