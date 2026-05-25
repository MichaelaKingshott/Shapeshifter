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

    void Start()
    {
        puzzleUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // OPEN PUZZLE
        if (playerInRange && !generatorStarted && !puzzleOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
        }

        // CLOSE WITH ESC
        if (puzzleOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePuzzle();
        }
    }

    public void OpenPuzzle()
    {
        puzzleOpen = true;

        puzzleUI.SetActive(true);

        // Hide popup while puzzle is open
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }

        // Play open sound
        if (audioSource != null && openSound != null)
        {
            audioSource.PlayOneShot(openSound);
        }

        // Pause game
        Time.timeScale = 0f;

        // Unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Disable pause menu
        if (pauseScript != null)
        {
            pauseScript.enabled = false;
        }

        // Disable camera controls
        if (cameraController != null)
        {
            cameraController.LockCameraControls(true);
        }
    }

    public void ClosePuzzle()
    {
        puzzleOpen = false;

        puzzleUI.SetActive(false);

        // Play close sound
        if (audioSource != null && closeSound != null)
        {
            audioSource.PlayOneShot(closeSound);
        }

        // Resume game
        Time.timeScale = 1f;

        // Lock mouse again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Re-enable pause menu
        if (pauseScript != null)
        {
            pauseScript.enabled = true;
        }

        // Re-enable camera controls
        if (cameraController != null)
        {
            cameraController.LockCameraControls(false);
        }

        // Show popup again if player still nearby
        if (playerInRange && !generatorStarted && popupText != null)
        {
            popupText.text = generatorMessage;
            popupText.gameObject.SetActive(true);
        }
    }

    public void ActivateGenerator()
    {
        if (generatorStarted)
            return;

        generatorStarted = true;

        // Play generator start sound
        if (audioSource != null && generatorStartSound != null)
        {
            audioSource.PlayOneShot(generatorStartSound);
        }

        // Turn on power
        if (PowerSystem.instance != null)
        {
            PowerSystem.instance.SetPower(true);
        }

        // Close puzzle after solving
        ClosePuzzle();

        // Hide popup permanently
        if (popupText != null)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Only show prompt if puzzle isn't open
            if (!puzzleOpen && !generatorStarted && popupText != null)
            {
                popupText.text = generatorMessage;
                popupText.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (popupText != null)
            {
                popupText.gameObject.SetActive(false);
            }

            // Safety close if player walks away
            if (puzzleOpen)
            {
                ClosePuzzle();
            }
        }
    }
}