using UnityEngine;

public class KeypadInteract : MonoBehaviour
{
    [Header("UI")]
    public GameObject keypadUI;
    public GameObject interactPrompt;

    [Header("Player")]
    [SerializeField] MonoBehaviour cameraScript;

    private bool playerNear = false;
    private bool keypadOpen = false;

    void Start()
    {
        keypadUI.SetActive(false);
        interactPrompt.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
    }

    void Update()
    {
        // OPEN KEYPAD
        if (playerNear && !keypadOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenKeypad();
        }

        // CLOSE WITH ESC
        if (keypadOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }

    public void OpenKeypad()
    {
        keypadOpen = true;

        keypadUI.SetActive(true);
        interactPrompt.SetActive(false);

        // Pause game
        Time.timeScale = 0f;

        // Disable camera look
        if (cameraScript != null)
        {
            cameraScript.enabled = false;
        }

        // Unlock mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        keypadOpen = false;

        keypadUI.SetActive(false);

        // Resume game
        Time.timeScale = 1f;

        // Re-enable camera
        if (cameraScript != null)
        {
            cameraScript.enabled = true;
        }

        // Lock mouse again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Show interact prompt again if still nearby
        if (playerNear)
        {
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;

            // Only show prompt if keypad isn't open
            if (!keypadOpen)
            {
                interactPrompt.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;

            interactPrompt.SetActive(false);

            // Safety close if player walks away
            if (keypadOpen)
            {
                CloseKeypad();
            }
        }
    }
}