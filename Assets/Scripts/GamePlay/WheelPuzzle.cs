using UnityEngine;
using System.Collections;

public class WheelPuzzle : MonoBehaviour
{
    [Header("Wheels")]
    public Wheel[] wheels;

    [Header("Water")]
    public Water water;
    public float solvedWaterHeight = 10f;

    [Tooltip("Time to wait before opening gates")]
    public float delayBeforeGateOpens = 3f;

    [Header("Flood Gate")]
    public FloodGate floodGate;

    [Header("Cutscene Cameras")]
    public GameObject mainCamera;
    public GameObject gateCamera;

    [Header("Cutscene Settings")]
    public float gateCameraTime = 3f;

    [Header("Player")]
    public GameObject activePlayer;

    private bool solved = false;

    public void CheckPuzzle()
    {
        if (solved) return;

        foreach (Wheel w in wheels)
        {
            if (!w.IsCorrect())
                return;
        }

        StartCoroutine(SolveSequence());
    }

    IEnumerator SolveSequence()
    {
        solved = true;

        Debug.Log("Puzzle Solved!");

        // Raise water immediately
        if (water != null)
            water.SetWaterHeight(solvedWaterHeight);

        // SHORT DELAY BEFORE CAMERA CUT
        yield return new WaitForSeconds(1f);

        // SWITCH TO GATE CAMERA
        if (mainCamera != null)
            mainCamera.SetActive(false);

        if (gateCamera != null)
            gateCamera.SetActive(true);

        // Freeze player
        if (activePlayer != null)
            activePlayer.SetActive(false);

        // Pause before gate opens
        yield return new WaitForSeconds(delayBeforeGateOpens);

        // Open gate
        if (floodGate != null)
            floodGate.OpenGate();

        // Hold cutaway
        yield return new WaitForSeconds(gateCameraTime);

        // Return to gameplay
        if (gateCamera != null)
            gateCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.SetActive(true);

        // Re-enable player
        if (activePlayer != null)
            activePlayer.SetActive(true);
    }
}
