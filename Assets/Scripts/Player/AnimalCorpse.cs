using UnityEngine;

public class AnimalCorpse : MonoBehaviour
{
    public AnimalForm animalType;

    private bool playerInRange;
    private ShapeshifterController player;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            player.UnlockForm(animalType);

            InteractionPromptUI.Instance.HidePrompt();

            Debug.Log("Consumed corpse and unlocked: " + animalType);

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