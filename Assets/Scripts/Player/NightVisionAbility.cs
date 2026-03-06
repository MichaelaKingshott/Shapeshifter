using UnityEngine;

public class NightVisionAbility : MonoBehaviour, IAnimalAbility
{
    private NightVisionController nightVision;

    void Awake()
    {
        nightVision = Camera.main.GetComponent<NightVisionController>();
    }

    public void OnFormActivated()
    {
        if (nightVision != null)
            nightVision.EnableNightVision(true);
    }

    public void OnFormDeactivated()
    {
        if (nightVision != null)
            nightVision.EnableNightVision(false);
    }
}
