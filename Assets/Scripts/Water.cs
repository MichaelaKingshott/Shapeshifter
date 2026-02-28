using UnityEngine;

public class Water : MonoBehaviour
{
    public float waterDrag = 3f;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearDamping = waterDrag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearDamping = 0f;
        }
    }
}





