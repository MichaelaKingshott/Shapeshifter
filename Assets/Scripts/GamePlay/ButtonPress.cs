using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    [Header("Target")]
    public MovingObject targetObject;
    public bool toggle = true;

    [Header("UI")]
    public GameObject interactPopup;

    [Header("Interaction")]
    public float interactDistance = 3f;

    private bool activated = false;
    private bool playerInRange = false;

    private Transform player;

    void Start()
    {
        if (interactPopup != null)
            interactPopup.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        // Extra safety distance check
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > interactDistance)
            {
                playerInRange = false;

                if (interactPopup != null)
                    interactPopup.SetActive(false);
            }
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        if (targetObject == null)
            return;

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

            if (interactPopup != null)
                interactPopup.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPopup != null)
                interactPopup.SetActive(false);
        }
    }
}