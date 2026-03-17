using UnityEngine;
using TMPro;

public class PlayerNoteReader : MonoBehaviour
{
    public float interactDistance = 3f;

    [SerializeField] GameObject notePanel;
    [SerializeField] TMP_Text noteText;

    public TMP_Text popupText;
    public string readMessage = "Press E to read note";

    bool reading = false;

    ShapeshifterController shapeshifter;

    void Start()
    {
        notePanel.SetActive(false);
        shapeshifter = FindFirstObjectByType<ShapeshifterController>();
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

        GameObject animal = shapeshifter.GetCurrentAnimalInstance();
        if (animal != null)
        {
            AnimalController controller = animal.GetComponent<AnimalController>();
            if (controller != null)
                controller.movementScript.enabled = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CloseNote()
    {
        notePanel.SetActive(false);
        reading = false;

        GameObject animal = shapeshifter.GetCurrentAnimalInstance();
        if (animal != null)
        {
            AnimalController controller = animal.GetComponent<AnimalController>();
            if (controller != null)
                controller.movementScript.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}