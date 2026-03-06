using UnityEngine;
using TMPro;

public class PlayerNoteReader : MonoBehaviour
{
    public float interactDistance = 3f;

    public MonoBehaviour playerMovement;
    public MonoBehaviour cameraLook;

    public GameObject notePanel;
    public TMP_Text noteText;

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

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            PaperNote note = hit.collider.GetComponent<PaperNote>();

            if (note != null && Input.GetKeyDown(KeyCode.E))
            {
                OpenNote(note);
            }
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