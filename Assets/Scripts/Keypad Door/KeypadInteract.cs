using UnityEngine;

public class KeypadInteract : MonoBehaviour
{
    public GameObject keypadUI;
    public GameObject interactPrompt;

    public CameraController cameraController;

    private bool playerNear = false;
    private bool keypadOpen = false;

    void Start()
    {
        keypadUI.SetActive(false);
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerNear && !keypadOpen && Input.GetKeyDown(KeyCode.E))
        {
            OpenKeypad();
        }

        if (keypadOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseKeypad();
        }
    }

    void OpenKeypad()
    {
        keypadUI.SetActive(true);
        interactPrompt.SetActive(false);

        keypadOpen = true;

        Time.timeScale = 0f;

        cameraController.LockCameraControls(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseKeypad()
    {
        keypadUI.SetActive(false);

        keypadOpen = false;

        Time.timeScale = 1f;

        cameraController.LockCameraControls(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            interactPrompt.SetActive(false);
        }
    }
}