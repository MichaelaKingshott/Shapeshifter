using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Transform[] waypoints;       // Assign in Inspector
    public float speed = 3f;
    public float reachThreshold = 0.2f;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;

        // Move toward the next waypoint
        transform.position += direction * speed * Time.deltaTime;

        // Optionally rotate toward the target
        transform.LookAt(target);

        // Check if close enough to move to the next point
        if (Vector3.Distance(transform.position, target.position) < reachThreshold)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length; // loops forever
        }
    }
}

