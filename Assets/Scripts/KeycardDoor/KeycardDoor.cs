using UnityEngine;

public class KeycardDoor : MonoBehaviour
{
    public Transform door;
    public float openHeight = 4f;
    public float speed = 2f;

    public KeycardType requiredKeycard;
    public PlayerInventory playerInventory;

    private bool playerInside = false;
    private bool opening = false;
    private Vector3 targetPosition;
    public DoorMessageUI doorMessage;
    public GameObject interactPrompt;

    void Start()
    {
        targetPosition = door.position + Vector3.up * openHeight;
        interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory.HasKeycard(requiredKeycard))
            {
                playerInventory.RemoveKeycard(requiredKeycard);
                opening = true;
            }
            else
            {
                doorMessage.ShowMessage(requiredKeycard + " keycard required");
            }
        }

        if (opening)
        {
            door.position = Vector3.MoveTowards(
                door.position,
                targetPosition,
                speed * Time.deltaTime
            );
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            interactPrompt.SetActive(false);
        }
    }
}