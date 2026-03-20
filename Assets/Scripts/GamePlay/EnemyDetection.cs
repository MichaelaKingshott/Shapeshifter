using UnityEngine;
using UnityEngine.UI;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionTime = 3f;
    public float loseDetectionSpeed = 1.5f;

    public SphereCollider detectionTrigger;
    public LayerMask playerMask;

    [Header("UI")]
    public Slider detectionSlider;

    [Header("Game Over")]
    public GameOver gameOverManager;

    private Transform player;
    private ShapeshifterController shapeshifter;

    private bool playerInside = false;
    private bool playerCaught = false;

    private float detectionTimer = 0f;

    private void Awake()
    {
        if (detectionTrigger == null)
            detectionTrigger = GetComponent<SphereCollider>();

        detectionTrigger.isTrigger = true;

        if (detectionSlider != null)
        {
            detectionSlider.minValue = 0;
            detectionSlider.maxValue = detectionTime;
            detectionSlider.value = 0;
            detectionSlider.gameObject.SetActive(false); // Hide at start
        }
    }

    private void Update()
    {
        if (playerCaught) return;

        if (playerInside)
        {
            // Check invisibility
            if (shapeshifter != null && shapeshifter.IsInvisible())
            {
                playerInside = false;
                return;
            }

            detectionTimer += Time.deltaTime;
        }
        else
        {
            detectionTimer -= Time.deltaTime * loseDetectionSpeed;
        }

        detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTime);

        if (detectionSlider != null)
        {
            detectionSlider.value = detectionTimer;

            Image fill = detectionSlider.fillRect.GetComponent<Image>();

            if (detectionTimer > detectionTime * 0.7f)
                fill.color = Color.red;       // Almost caught
            else
                fill.color = Color.yellow;    // Detecting
        }

        // Hide bar when fully drained
        if (!playerInside && detectionTimer <= 0 && detectionSlider != null)
            detectionSlider.gameObject.SetActive(false);

        if (detectionTimer >= detectionTime)
        {
            PlayerCaught();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsPlayer(other.gameObject) || playerCaught) return;

        // Avoid setting playerInside to true for multiple enemies
        if (!playerInside)
        {
            playerInside = true;
            player = other.transform;

            shapeshifter = player.GetComponent<ShapeshifterController>();

            if (detectionSlider != null)
                detectionSlider.gameObject.SetActive(true); // Show bar
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsPlayer(other.gameObject)) return;

        playerInside = false;
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerMask.value & (1 << obj.layer)) != 0;
    }

    private void PlayerCaught()
    {
        if (playerCaught) return;

        playerCaught = true;

        Debug.Log("PLAYER CAUGHT!");

        if (gameOverManager != null)
            gameOverManager.Caught();
    }
}
