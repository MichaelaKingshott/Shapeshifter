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

    [Header("Visuals")]
    public Renderer buttonRenderer;
    public Material onMaterial;   // Green
    public Material offMaterial;  // Red

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;

    private bool activated = false;
    private bool playerInRange = false;

    private Transform player;

    private InteractableHighlight highlight;

    void Start()
    {
        if (interactPopup != null)
            interactPopup.SetActive(false);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;

        highlight = GetComponentInChildren<InteractableHighlight>();

        UpdateVisual();
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);

            if (distance > interactDistance)
            {
                playerInRange = false;

                if (interactPopup != null)
                    interactPopup.SetActive(false);

                if (highlight != null)
                    highlight.HideHighlight();
            }
        }

        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PressButton();
        }
    }

    void UpdateVisual()
    {
        if (buttonRenderer == null)
            return;

        buttonRenderer.material = activated ? onMaterial : offMaterial;
    }

    public void PressButton()
    {
        if (targetObject == null)
            return;

        // PLAY BUTTON SOUND
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }

        if (toggle)
        {
            activated = !activated;
        }
        else
        {
            activated = true;
        }

        targetObject.SetActiveState(activated);

        UpdateVisual();

        Debug.Log("Button pressed!");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPopup != null)
                interactPopup.SetActive(true);

            if (highlight != null)
                highlight.ShowHighlight();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPopup != null)
                interactPopup.SetActive(false);

            if (highlight != null)
                highlight.HideHighlight();
        }
    }
}