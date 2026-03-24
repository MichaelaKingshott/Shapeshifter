using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyDetection : MonoBehaviour
{
    public LayerMask playerMask;
    public DetectionManager detectionManager;

    private bool isDetecting = false;
    private ShapeshifterController shapeshifter;

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void Update()
    {
        if (!isDetecting) return;

        if (shapeshifter != null && shapeshifter.IsInvisible())
        {
            StopDetection();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other)) return;

        shapeshifter = other.GetComponent<ShapeshifterController>();

        StartDetection();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other)) return;

        StopDetection();
    }

    private void StartDetection()
    {
        if (isDetecting) return;

        isDetecting = true;
        detectionManager.StartDetecting();
    }

    private void StopDetection()
    {
        if (!isDetecting) return;

        isDetecting = false;
        detectionManager.StopDetecting();
    }

    private bool IsPlayer(Collider col)
    {
        return ((1 << col.gameObject.layer) & playerMask) != 0;
    }
}
