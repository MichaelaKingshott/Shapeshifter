using UnityEngine;

public class AnimalCorpse : MonoBehaviour
{
    public AnimalForm animalType;

    private bool playerInRange;
    private ShapeshifterController player;

    void Start()
    {
        // If already consumed before → destroy immediately
        if (GameManager.Instance.consumedCorpses.Contains(animalType))
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Unlock form
            player.UnlockForm(animalType);

            // Mark as consumed
            GameManager.Instance.consumedCorpses.Add(animalType);

            // ✅ SET CHECKPOINT HERE
            GameManager.Instance.checkpointPosition = player.transform.position;
            GameManager.Instance.hasCheckpoint = true;

            InteractionPromptUI.Instance.HidePrompt();

            Debug.Log("Consumed corpse, unlocked: " + animalType + " + CHECKPOINT SET");

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShapeshifterController controller = other.GetComponentInParent<ShapeshifterController>();

        if (controller != null)
        {
            player = controller;
            playerInRange = true;

            InteractionPromptUI.Instance.ShowPrompt("Press E to consume");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ShapeshifterController controller = other.GetComponentInParent<ShapeshifterController>();

        if (controller == player)
        {
            playerInRange = false;
            player = null;

            InteractionPromptUI.Instance.HidePrompt();
        }
    }
}