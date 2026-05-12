using UnityEngine;

public class AnimalCorpse : MonoBehaviour
{
    public AnimalForm animalType;

    private bool playerInRange;
    private ShapeshifterController player;

    private InteractableHighlight highlight;

    void Start()
    {
        highlight = GetComponentInChildren<InteractableHighlight>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            player.UnlockForm(animalType);

            // Set checkpoint
            player.SetCheckpoint(transform.position);

            InteractionPromptUI.Instance.HidePrompt();

            // Hide highlight before destroy
            if (highlight != null)
                highlight.HideHighlight();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ShapeshifterController controller =
            other.GetComponentInParent<ShapeshifterController>();

        if (controller != null)
        {
            player = controller;
            playerInRange = true;

            InteractionPromptUI.Instance.ShowPrompt("Press E to consume");

            // SHOW HIGHLIGHT
            if (highlight != null)
                highlight.ShowHighlight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ShapeshifterController controller =
            other.GetComponentInParent<ShapeshifterController>();

        if (controller == player)
        {
            playerInRange = false;
            player = null;

            InteractionPromptUI.Instance.HidePrompt();

            // HIDE HIGHLIGHT
            if (highlight != null)
                highlight.HideHighlight();
        }
    }
}