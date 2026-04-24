using UnityEngine;
using System.Collections;

public class Valve : MonoBehaviour
{
    public Water water;

    [Header("Water Height")]
    public float targetHeight;

    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Rotation")]
    public float rotateSpeed = 180f;
    public float turnDuration = 1.5f;

    [Header("UI")]
    public GameObject interactPrompt;

    private bool playerInRange = false;
    private bool isTurning = false;
    private bool hasBeenUsed = false; // NEW

    void Start()
    {
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (isTurning)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        }

        // Prevent reuse here
        if (playerInRange && Input.GetKeyDown(interactKey) && !isTurning && !hasBeenUsed)
        {
            Activate();
        }
    }

    void Activate()
    {
        hasBeenUsed = true; // mark as used immediately
        StartCoroutine(TurnValve());
    }

    IEnumerator TurnValve()
    {
        isTurning = true;

        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        float timer = 0f;

        while (timer < turnDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        isTurning = false;

        if (water != null)
        {
            water.SetWaterHeight(targetHeight);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenUsed)
        {
            playerInRange = true;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }
        }
    }
}