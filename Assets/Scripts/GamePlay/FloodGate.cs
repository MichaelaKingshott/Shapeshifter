using UnityEngine;

public class FloodGate : MonoBehaviour
{
    [Header("Gate Transforms (Pivots!)")]
    public Transform leftGate;
    public Transform rightGate;

    [Header("Settings")]
    public float openAngle = 90f;
    public float speed = 2f;

    private bool opening = false;

    private Quaternion leftClosedRot;
    private Quaternion rightClosedRot;

    private Quaternion leftOpenRot;
    private Quaternion rightOpenRot;

    void Start()
    {
        // Store starting rotations
        leftClosedRot = leftGate.rotation;
        rightClosedRot = rightGate.rotation;

        // Calculate open rotations
        leftOpenRot = leftClosedRot * Quaternion.Euler(0, -openAngle, 0);
        rightOpenRot = rightClosedRot * Quaternion.Euler(0, openAngle, 0);
    }

    void Update()
    {
        if (!opening) return;

        leftGate.rotation = Quaternion.Slerp(
            leftGate.rotation,
            leftOpenRot,
            Time.deltaTime * speed
        );

        rightGate.rotation = Quaternion.Slerp(
            rightGate.rotation,
            rightOpenRot,
            Time.deltaTime * speed
        );
    }

    public void OpenGate()
    {
        opening = true;
    }
}
