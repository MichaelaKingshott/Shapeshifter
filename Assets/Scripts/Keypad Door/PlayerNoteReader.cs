using UnityEngine;
using TMPro;

public class PlayerNoteReader : MonoBehaviour
{
    public float interactDistance = 3f;

    public MonoBehaviour playerMovement;
    public MonoBehaviour cameraLook;

    [SerializeField] GameObject notePanel;
    [SerializeField] TMP_Text noteText;

    public TMP_Text popupText; // assign in inspector
    public string readMessage = "Press E to read note";

    bool reading = false;

    void Start()
    {
        notePanel.SetActive(false);
    }

    void Update()
    {
        if (reading)
        {
            if (Input.GetKeyDown(KeyCode.E))
                CloseNote();

            return;
        }

        RaycastHit hit;
        bool lookingAtNote = false;

        if (Physics.Raycast(transform.position, transform.forward, out hit, interactDistance))
        {
            PaperNote note = hit.collider.GetComponent<PaperNote>();

            if (note != null)
            {
                popupText.text = readMessage;
                popupText.gameObject.SetActive(true);
                lookingAtNote = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    OpenNote(note);
                    popupText.gameObject.SetActive(false);
                }
            }
        }

        if (!lookingAtNote)
        {
            popupText.gameObject.SetActive(false);
        }
    }

    void OpenNote(PaperNote note)
    {
        notePanel.SetActive(true);
        noteText.text = note.noteText;
        reading = true;

        playerMovement.enabled = false;
        cameraLook.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseNote()
    {
        notePanel.SetActive(false);
        reading = false;

        playerMovement.enabled = true;
        cameraLook.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
