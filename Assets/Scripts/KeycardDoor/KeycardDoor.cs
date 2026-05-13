using UnityEngine;

public class KeycardDoor : MonoBehaviour
{
    [Header("Door")]
    public SlidingDoor slidingDoor;

    [Header("Keycard")]
    public KeycardType requiredKeycard;
    public PlayerInventory playerInventory;

    [Header("UI")]
    public DoorMessageUI doorMessage;
    public GameObject interactPrompt;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deniedSound;

    private bool playerInside = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            if (playerInventory != null &&
                playerInventory.HasKeycard(requiredKeycard))
            {
                playerInventory.RemoveKeycard(requiredKeycard);

                // OPEN THE DOOR
                if (slidingDoor != null)
                    slidingDoor.OpenDoor();
            }
            else
            {
                if (doorMessage != null)
                    doorMessage.ShowMessage(requiredKeycard + " keycard required");

                // PLAY DENIED SOUND
                if (audioSource != null && deniedSound != null)
                    audioSource.PlayOneShot(deniedSound);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}