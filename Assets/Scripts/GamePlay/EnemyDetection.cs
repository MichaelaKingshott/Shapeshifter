using UnityEngine;
using System.Collections;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionTime = 3f;
    public SphereCollider detectionTrigger;
    public LayerMask playerMask;

    private Transform player;
    private ShapeshifterController shapeshifter;  // NEW
    private bool playerInside = false;
    private Coroutine detectionRoutine;

    private void Awake()
    {
        if (detectionTrigger == null)
            detectionTrigger = GetComponent<SphereCollider>();

        detectionTrigger.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other.gameObject)) return;

        playerInside = true;
        player = other.transform;

        shapeshifter = player.GetComponent<ShapeshifterController>();

        if (detectionRoutine == null)
            detectionRoutine = StartCoroutine(DetectionCountdown());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other.gameObject)) return;

        playerInside = false;

        if (detectionRoutine != null)
        {
            StopCoroutine(detectionRoutine);
            detectionRoutine = null;
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerMask.value & (1 << obj.layer)) != 0;
    }

    private IEnumerator DetectionCountdown()
    {
        float timer = detectionTime;

        while (timer > 0f)
        {
            if (!playerInside)
            {
                detectionRoutine = null;
                yield break;
            }

            // NEW: Invisible check from any animal form
            if (shapeshifter != null && shapeshifter.IsInvisible())
            {
                detectionRoutine = null;
                yield break;
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        PlayerCaught();
        detectionRoutine = null;
    }

    private void PlayerCaught()
    {
        Debug.Log("PLAYER CAUGHT!");
        // TODO: Game over, respawn, etc.
    }
}


