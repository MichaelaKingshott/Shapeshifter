using UnityEngine;
using TMPro;

public class NoteInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject promptUI;
    public GameObject noteUI;

    [Header("Note Content")]
    [TextArea(3, 10)]
    public string noteText;

    public TextMeshProUGUI noteTextUI;

    [Header("Player")]
    public CameraController cameraController;

    private bool playerInRange = false;
    private bool noteOpen = false;

    void Start()
    {
        promptUI.SetActive(false);
        noteUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerInRange && !noteOpen)
            {
                OpenNote();
            }
            else if (noteOpen)
            {
                CloseNote();
            }
        }
    }

    void OpenNote()
    {
        noteOpen = true;

        noteUI.SetActive(true);
        promptUI.SetActive(false);

        noteTextUI.text = noteText;

        if (cameraController != null)
            cameraController.LockCameraControls(true);

        Time.timeScale = 0f;
    }

    public void CloseNote()
    {
        noteOpen = false;

        noteUI.SetActive(false);

        if (cameraController != null)
            cameraController.LockCameraControls(false);

        Time.timeScale = 1f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (!noteOpen)
                promptUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            promptUI.SetActive(false);
        }
    }
}
