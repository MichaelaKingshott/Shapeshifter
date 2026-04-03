using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public MovingObject targetObject;
    public bool toggle = true;

    private bool activated = false;

    public GameObject interactPopup;
    private bool playerInRange = false;

    void Start()
    {
        interactPopup.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        if (targetObject == null) return;

        if (toggle)
        {
            activated = !activated;
            targetObject.SetActiveState(activated);
        }
        else
        {
            targetObject.SetActiveState(true);
        }

        Debug.Log("Button pressed!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            interactPopup.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            interactPopup.SetActive(false);
        }
    }
}