using UnityEngine;
using TMPro;

public class PlayerNoteReader : MonoBehaviour
{
    public Camera cam;
    public float interactDistance = 3f;
    public float sphereRadius = 0.15f;

    [SerializeField] GameObject notePanel;
    [SerializeField] TMP_Text noteText;

    public TMP_Text popupText;
    public string readMessage = "Press E to read note";

    bool reading = false;

    ShapeshifterController shapeshifter;
    public CameraController cameraController;

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

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        bool lookingAtNote = false;

        if (Physics.SphereCast(ray, sphereRadius, out RaycastHit hit, interactDistance))
        {
            PaperNote note = hit.collider.GetComponentInParent<PaperNote>();

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

    public void TryOpenNoteFromTongue(PaperNote note)
    {
        if (reading) return;
        OpenNote(note);
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

        if (cameraController != null)
            cameraController.LockCameraControls(true);

        Time.timeScale = 0f;
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

        if (cameraController != null)
            cameraController.LockCameraControls(false);

        Time.timeScale = 1f;
    }
}