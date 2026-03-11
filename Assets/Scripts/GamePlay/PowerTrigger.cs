using UnityEngine;

public class PowerTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PowerSystem.instance.TriggerBlackout();

            Destroy(gameObject);
        }
    }
}
