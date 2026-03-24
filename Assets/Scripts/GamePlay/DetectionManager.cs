using UnityEngine;
using UnityEngine.UI;

public class DetectionManager : MonoBehaviour
{
    public float detectionTime = 3f;
    public Slider detectionSlider;
    public GameOver gameOverManager;

    private float detectionTimer = 0f;
    private bool playerCaught = false;

    private int enemiesDetecting = 0;

    private Image fillImage;

    private void Start()
    {
        if (detectionSlider != null)
        {
            detectionSlider.minValue = 0;
            detectionSlider.maxValue = detectionTime;
            detectionSlider.value = 0;
            detectionSlider.gameObject.SetActive(false);

            fillImage = detectionSlider.fillRect.GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (playerCaught) return;

        if (enemiesDetecting > 0)
        {
            detectionTimer += Time.deltaTime * enemiesDetecting;
        }
        else
        {
            detectionTimer -= Time.deltaTime * 1.5f;
        }

        detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTime);

        UpdateUI();

        if (detectionTimer >= detectionTime)
        {
            TriggerGameOver();
        }
    }

    private void UpdateUI()
    {
        if (detectionSlider == null) return;

        detectionSlider.value = detectionTimer;
        detectionSlider.gameObject.SetActive(detectionTimer > 0.01f);

        if (fillImage != null)
        {
            fillImage.color = detectionTimer > detectionTime * 0.7f ? Color.red : Color.yellow;
        }
    }

    public void StartDetecting()
    {
        enemiesDetecting++;
    }

    public void StopDetecting()
    {
        enemiesDetecting = Mathf.Max(0, enemiesDetecting - 1);
    }

    public void TriggerGameOver()
    {
        if (playerCaught) return;

        playerCaught = true;

        if (gameOverManager != null)
            gameOverManager.Caught();
    }
}