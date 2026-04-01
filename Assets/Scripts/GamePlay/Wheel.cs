using UnityEngine;
using System.Collections;

public class Wheel : MonoBehaviour
{
    [Header("Interaction")]
    public KeyCode interactKey = KeyCode.E;

    [Header("Rotation Settings")]
    public float stepAngle = 90f;     // 90 = 4 positions, 45 = 8 positions
    public float turnSpeed = 5f;

    [Tooltip("Choose the axis the wheel spins on")]
    public Vector3 rotationAxis = Vector3.forward; // change to right/forward if needed

    [Header("Correct Position")]
    public int correctStep = 1;

    [Header("References")]
    public WheelPuzzle puzzleManager;
    public GameObject interactPrompt;

    [Header("Visual Feedback")]
    public Renderer wheelRenderer;
    public Color defaultColor = Color.white;
    public Color correctColor = Color.green;

    private bool playerInRange = false;
    private bool isRotating = false;

    private int currentStep = 0;
    private float targetAngle = 0f;

    void Start()
    {
        // Snap to nearest valid step on start
        float angle = GetCurrentAngle();
        currentStep = Mathf.RoundToInt(angle / stepAngle);

        targetAngle = currentStep * stepAngle;
        SetRotation(targetAngle);

        if (interactPrompt != null)
            interactPrompt.SetActive(false);

        if (wheelRenderer != null)
            wheelRenderer.material.color = defaultColor;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey) && !isRotating && !IsCorrect())
        {
            RotateStep();
        }

        UpdateColor();
    }

    void RotateStep()
    {
        currentStep++;

        int maxSteps = Mathf.RoundToInt(360f / stepAngle);
        if (currentStep >= maxSteps)
            currentStep = 0;

        targetAngle = currentStep * stepAngle;

        StartCoroutine(RotateToTarget());

        if (puzzleManager != null)
            puzzleManager.CheckPuzzle();
    }

    IEnumerator RotateToTarget()
    {
        isRotating = true;

        while (Mathf.Abs(Mathf.DeltaAngle(GetCurrentAngle(), targetAngle)) > 0.5f)
        {
            float newAngle = Mathf.LerpAngle(GetCurrentAngle(), targetAngle, Time.deltaTime * turnSpeed);
            SetRotation(newAngle);
            yield return null;
        }

        SetRotation(targetAngle);
        isRotating = false;
    }

    float GetCurrentAngle()
    {
        Vector3 euler = transform.localEulerAngles;

        if (rotationAxis == Vector3.forward)
            return euler.z;

        if (rotationAxis == Vector3.right)
            return euler.x;

        return euler.y;
    }

    void SetRotation(float angle)
    {
        Vector3 euler = transform.localEulerAngles;

        if (rotationAxis == Vector3.forward)
            transform.localEulerAngles = new Vector3(euler.x, euler.y, angle);

        else if (rotationAxis == Vector3.right)
            transform.localEulerAngles = new Vector3(angle, euler.y, euler.z);

        else
            transform.localEulerAngles = new Vector3(euler.x, angle, euler.z);
    }

    void UpdateColor()
    {
        if (wheelRenderer == null) return;

        if (IsCorrect())
            wheelRenderer.material.color = correctColor;
        else
            wheelRenderer.material.color = defaultColor;
    }

    public bool IsCorrect()
    {
        return currentStep == correctStep;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }
}