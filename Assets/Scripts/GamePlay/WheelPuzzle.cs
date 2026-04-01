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

        // 1️⃣ Raise water first
        if (water != null)
            water.SetWaterHeight(solvedWaterHeight);

        // 2️⃣ Wait (gives time for water to rise)
        yield return new WaitForSeconds(delayBeforeGateOpens);

        // 3️⃣ Open gates AFTER delay
        if (floodGate != null)
            floodGate.OpenGate();
    }
}
