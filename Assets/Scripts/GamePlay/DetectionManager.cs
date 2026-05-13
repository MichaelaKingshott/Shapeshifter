using UnityEngine;
using UnityEngine.UI;

public class DetectionManager : MonoBehaviour
{
    [Header("Detection")]
    public float detectionTime = 3f;
    public Slider detectionSlider;
    public GameOver gameOverManager;

    [Header("Detection Music")]
    public AudioSource musicSource;
    public AudioClip detectionMusic;
    public float fadeSpeed = 2f;

    private float detectionTimer = 0f;
    private bool playerCaught = false;

    private int enemiesDetecting = 0;

    private Image fillImage;

    private ShapeshifterController player;

    private bool musicPlaying = false;

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

        // MUSIC SETUP
        if (musicSource != null)
        {
            musicSource.clip = detectionMusic;
            musicSource.loop = true;
            musicSource.volume = 0f;
        }

        // RESPawn hookup
        player = FindFirstObjectByType<ShapeshifterController>();

        if (player != null)
        {
            player.OnRespawn += ResetDetection;
        }
    }

    private void Update()
    {
        if (playerCaught) return;

        if (enemiesDetecting > 0)
        {
            detectionTimer += Time.deltaTime * enemiesDetecting;

            StartMusic();
        }
        else
        {
            detectionTimer -= Time.deltaTime * 1.5f;

            StopMusic();
        }

        detectionTimer = Mathf.Clamp(detectionTimer, 0, detectionTime);

        UpdateUI();
        UpdateMusicFade();

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
            fillImage.color =
                detectionTimer > detectionTime * 0.7f
                ? Color.red
                : Color.yellow;
        }
    }

    void StartMusic()
    {
        if (musicSource == null || detectionMusic == null)
            return;

        if (!musicPlaying)
        {
            musicPlaying = true;

            if (!musicSource.isPlaying)
                musicSource.Play();
        }
    }

    void StopMusic()
    {
        musicPlaying = false;
    }

    void UpdateMusicFade()
    {
        if (musicSource == null)
            return;

        float targetVolume = musicPlaying ? 1f : 0f;

        musicSource.volume = Mathf.Lerp(
            musicSource.volume,
            targetVolume,
            Time.deltaTime * fadeSpeed
        );

        // STOP completely when nearly silent
        if (!musicPlaying && musicSource.volume < 0.01f)
        {
            musicSource.Stop();
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

    public void ResetDetection()
    {
        detectionTimer = 0f;
        enemiesDetecting = 0;
        playerCaught = false;

        musicPlaying = false;

        if (musicSource != null)
        {
            musicSource.Stop();
            musicSource.volume = 0f;
        }

        if (detectionSlider != null)
        {
            detectionSlider.value = 0;
            detectionSlider.gameObject.SetActive(false);
        }
    }
}