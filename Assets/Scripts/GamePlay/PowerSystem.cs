using UnityEngine;
using System.Collections;

public class PowerSystem : MonoBehaviour
{
    public static PowerSystem instance;

    [Header("Lights")]
    public Light[] lights;

    [Header("Vents")]
    public VentBlocker[] vents;

    [Header("Cutscene Cameras")]
    public GameObject mainCamera;
    public GameObject ventCamera;

    [Header("Cutscene Settings")]
    public float ventCameraTime = 3f;
    public float delayBeforeVentOpens = 1f;

    [Header("Player")]
    public GameObject activePlayer;

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

        // Stop pending vent sequence
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

        DestroyBlackoutEnemies();
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
    // VENT OPENING + CUTSCENE
    // ---------------------------
    private IEnumerator OpenVentsDelayed()
    {
        // Wait after power returns
        yield return new WaitForSeconds(0.5f);

        // SWITCH CAMERA IMMEDIATELY
        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (ventCamera != null)
            ventCamera.SetActive(true);

        // Freeze player AFTER camera switch
        if (activePlayer != null)
            activePlayer.SetActive(false);

        // Small pause so player sees the vent first
        yield return new WaitForSeconds(delayBeforeVentOpens);

        // Open vents
        foreach (VentBlocker v in vents)
        {
            if (v != null)
                v.SetOpen(true);
        }

        // Let player watch the vent open
        yield return new WaitForSeconds(ventCameraTime);

        // Return to gameplay
        if (ventCamera != null)
            ventCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.SetActive(true);

        // Re-enable player
        if (activePlayer != null)
            activePlayer.SetActive(true);
    }
}
