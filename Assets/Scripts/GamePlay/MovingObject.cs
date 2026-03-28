using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2f;

    private Vector3 target;
    private bool active = false;

    void Start()
    {
        // Start moving toward B initially
        target = pointB.position;
    }

    void Update()
    {
        if (!active) return;

        // Move toward target
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime
        );

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            // Swap target
            target = (target == pointA.position) ? pointB.position : pointA.position;
        }
    }

    public void SetActiveState(bool state)
    {
        active = state;
    }
}