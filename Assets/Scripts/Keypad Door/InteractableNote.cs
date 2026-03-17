using UnityEngine;
using TMPro;

public class InteractableNote : MonoBehaviour
{
    [TextArea(5, 10)]
    public string noteText;

    [Header("UI")]
    public GameObject notePanel;
    public TMP_Text noteUIText;
    public TMP_Text popupText;

    public string readMessage = "Press E to read note";

    private bool playerInRange = false;
    private bool reading = false;

    void Update()
    {
        if (playerInRange && !reading && Input.GetKeyDown(KeyCode.E))
        {
            OpenNote();
        }

        if (reading && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseNote();
        }
    }

    void OpenNote()
    {
        reading = true;

        notePanel.SetActive(true);
        noteUIText.text = noteText;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f; // match generator behavior
    }

    void CloseNote()
    {
        reading = false;

        notePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;

        popupText.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            popupText.text = readMessage;
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