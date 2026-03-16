using UnityEngine;
using TMPro;

public class GeneratorLever : MonoBehaviour
{
    private bool playerInRange = false;
    private bool generatorStarted = false;

    public GameObject puzzleUI;
    public Collider myCollider;

    public CameraController cameraController;

    public TMP_Text popupText;
    public string generatorMessage = "Press E to start Generator";

    void Update()
    {
        if (playerInRange && !generatorStarted && Input.GetKeyDown(KeyCode.E))
        {
            OpenPuzzle();
        }
    }

    void OpenPuzzle()
    {
        puzzleUI.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0f;

        cameraController.LockCameraControls(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ActivateGenerator()
    {
        if (generatorStarted) return;

        generatorStarted = true;

        PowerSystem.instance.SetPower(true);

        ClosePuzzle();
    }

    void ClosePuzzle()
    {
        puzzleUI.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1f;
                
        cameraController.LockCameraControls(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        myCollider.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            popupText.text = generatorMessage;
            popupText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
            popupText.gameObject.SetActive(false);
        }
    }
}
