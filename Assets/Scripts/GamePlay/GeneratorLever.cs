using UnityEngine;
using TMPro;

public class GeneratorLever : MonoBehaviour
{
    [Header("References")]
    [SerializeField] MonoBehaviour pauseScript;
    public GameObject puzzleUI;
    public CameraController cameraController;
    public TMP_Text popupText;

    [Header("Settings")]
    public string generatorMessage = "Press E to start Generator";

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip generatorStartSound;

    private bool playerInRange = false;
    private bool generatorStarted = false;
    private bool puzzleOpen = false;

    void Update()
    {
        // Open puzzle
        if (playerInRange && !generatorStarted && !puzzleOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
        }

        // Close puzzle with ESC
        if (puzzleOpen && Input.GetKeyDown(KeyCode.E))
        {
            ClosePuzzle();
        }
    }

    void OpenPuzzle()
    {
        puzzleOpen = true;

        puzzleUI.SetActive(true);

        // PLAY OPEN SOUND
        if (audioSource != null && openSound != null)
            audioSource.PlayOneShot(openSound);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        if (pauseScript != null)
            pauseScript.enabled = false;

        if (cameraController != null)
            cameraController.LockCameraControls(true);
    }

    public void ActivateGenerator()
    {
        if (generatorStarted) return;

        generatorStarted = true;

        // PLAY GENERATOR START SOUND
        if (audioSource != null && generatorStartSound != null)
            audioSource.PlayOneShot(generatorStartSound);

        PowerSystem.instance.SetPower(true);

        ClosePuzzle();
    }

    void ClosePuzzle()
    {
        puzzleOpen = false;

        puzzleUI.SetActive(false);

        // PLAY CLOSE SOUND
        if (audioSource != null && closeSound != null)
            audioSource.PlayOneShot(closeSound);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        if (pauseScript != null)
            pauseScript.enabled = true;

        if (cameraController != null)
            cameraController.LockCameraControls(false);

        popupText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            popupText.text = generatorMessage;
            popupText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            popupText.gameObject.SetActive(false);
        }
    }
}

