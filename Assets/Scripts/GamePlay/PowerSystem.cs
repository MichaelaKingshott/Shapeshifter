using UnityEngine;
using System.Collections;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem instance;

    public Light[] lights;
    public VentBlocker[] vents;

    private Coroutine ventRoutine;

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

        // Stop any pending vent opening
        if (ventRoutine != null)
        {
            StopCoroutine(ventRoutine);
            ventRoutine = null;
        }

        if (state)
        {
            ventRoutine = StartCoroutine(OpenVentsDelayed());
        }
        else
        {
            foreach (VentBlocker v in vents)
            {
                v.SetOpen(false);
            }
        }
    }

    public void TriggerBlackout()
    {
        StartCoroutine(FlickerAndShutdown());
    }

    IEnumerator FlickerAndShutdown()
    {
        for (int i = 0; i < 5; i++)
        {
            SetPower(false);
            yield return new WaitForSeconds(0.15f);

            SetPower(true);
            yield return new WaitForSeconds(0.15f);
        }

        SetPower(false);
    }

    IEnumerator OpenVentsDelayed()
    {
        yield return new WaitForSeconds(1.5f);

        foreach (VentBlocker v in vents)
        {
            v.SetOpen(true);
        }
    }
}
