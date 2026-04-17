using UnityEngine;
using System.Collections;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem instance;

    [Header("Lights")]
    public Light[] lights;

    [Header("Vents")]
    public VentBlocker[] vents;

    private Coroutine ventRoutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    // ---------------------------
    // POWER CONTROL
    // ---------------------------
    public void SetPower(bool state)
    {
        foreach (Light l in lights)
        {
            if (l != null)
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
                if (v != null)
                    v.SetOpen(false);
            }
        }
    }

    // ---------------------------
    // BLACKOUT ENTRY POINT
    // ---------------------------
    public void TriggerBlackout()
    {
        StartCoroutine(FlickerAndShutdown());
    }

    // ---------------------------
    // BLACKOUT SEQUENCE
    // ---------------------------
    private IEnumerator FlickerAndShutdown()
    {
        for (int i = 0; i < 5; i++)
        {
            SetPower(false);
            yield return new WaitForSeconds(0.15f);

            SetPower(true);
            yield return new WaitForSeconds(0.15f);
        }

        SetPower(false);

        DestroyBlackoutEnemies(); // 👈 enemies removed here
    }

    // ---------------------------
    // ENEMY REMOVAL
    // ---------------------------
    private void DestroyBlackoutEnemies()
    {
        BlackoutDestroyable[] enemies =
            FindObjectsByType<BlackoutDestroyable>(FindObjectsSortMode.None);

        foreach (BlackoutDestroyable enemy in enemies)
        {
            if (enemy != null && enemy.destroyOnBlackout)
            {
                Destroy(enemy.gameObject);
            }
        }
    }

    // ---------------------------
    // VENTS
    // ---------------------------
    private IEnumerator OpenVentsDelayed()
    {
        yield return new WaitForSeconds(1.5f);

        foreach (VentBlocker v in vents)
        {
            if (v != null)
                v.SetOpen(true);
        }
    }
}
