using UnityEngine;
using System.Collections;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem instance;

    public Light[] lights;

    void Awake()
    {
        instance = this;
    }

    public void SetPower(bool state)
    {
        foreach (Light l in lights)
        {
            l.enabled = state;
        }
    }

    public void TriggerBlackout()
    {
        StartCoroutine(FlickerAndShutdown());
    }

    IEnumerator FlickerAndShutdown()
    {
        for(int i = 0; i < 5; i++)
        {
            SetPower(false);
            yield return new WaitForSeconds(0.15f);

            SetPower(true);
            yield return new WaitForSeconds(0.15f);
        }

        SetPower(false);
    }
}
