using UnityEngine;
using System.Collections;

public class ChameleonMovement : MonoBehaviour, IAnimalAbility, IAnimalForm
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float turnSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Invisibility Ability")]
    public float invisDuration = 3f;
    public float cooldown = 5f;

    [Header("UI")]
    public float maxEnergy = 100f;

    [Header("Materials")]
    public Material normalMaterial;
    public Material outlineMaterial;

    [Header("Animation")]
    public float moveThreshold = 0.1f;

    [Header("Water")]
    public float sinkForce = 6f;

    public Collider groundCollider;

    private Renderer[] renderers;
    private Rigidbody rb;
    private Animator anim;
    private Camera cam;

    private bool isInvisible = false;
    private bool canUseInvisibility = true;
    private bool isGrounded = true;
    private bool isInWater = false;

    private Vector3 moveInput;
    private bool jumpPressed;

    private float currentEnergy;

    private ChameleonSliderUI sliderUI;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        renderers = GetComponentsInChildren<Renderer>();

        FindCamera();

        currentEnergy = maxEnergy;

        sliderUI = FindFirstObjectByType<ChameleonSliderUI>();

        if (sliderUI != null)
        {
            sliderUI.slider.maxValue = maxEnergy;
            sliderUI.slider.value = maxEnergy;
            sliderUI.Hide();
        }

        if (groundCollider == null)
        {
            Collider[] cols = GetComponentsInChildren<Collider>();

            if (cols.Length > 0)
                groundCollider = cols[0];
        }
    }

    void Update()
    {
        if (cam == null)
        {
            FindCamera();

            if (cam == null)
                return;
        }

        ReadInput();
        HandleInvisibility();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (cam == null)
            return;

        Move();
        HandleJump();
        HandleWater();
    }

    void FindCamera()
    {
        if (Camera.main != null)
            cam = Camera.main;
    }

    void ReadInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cam.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        moveInput =
            (camForward * v + camRight * h).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
            jumpPressed = true;
    }

    void Move()
    {
        if (isInWater)
        {
            rb.linearVelocity = new Vector3(
                0f,
                rb.linearVelocity.y,
                0f
            );

            return;
        }

        rb.linearVelocity = new Vector3(
            moveInput.x * moveSpeed,
            rb.linearVelocity.y,
            moveInput.z * moveSpeed
        );

        if (moveInput.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(moveInput);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                turnSpeed * Time.fixedDeltaTime
            );
        }
    }

    void HandleJump()
    {
        if (!jumpPressed)
            return;

        jumpPressed = false;

        if (!isGrounded || isInWater)
            return;

        rb.AddForce(
            Vector3.up * jumpForce,
            ForceMode.Impulse
        );

        isGrounded = false;

        anim.SetTrigger("Jump");
    }

    void HandleWater()
    {
        if (!isInWater)
            return;

        rb.AddForce(
            Vector3.down * sinkForce,
            ForceMode.Acceleration
        );
    }

    void UpdateAnimations()
    {
        Vector3 horizontalVel =
            new Vector3(
                rb.linearVelocity.x,
                0f,
                rb.linearVelocity.z
            );

        float speed = horizontalVel.magnitude;

        anim.SetFloat(
            "Speed",
            speed < moveThreshold ? 0f : speed
        );

        anim.SetBool("isGrounded", isGrounded);
    }

    void HandleInvisibility()
    {
        if (Input.GetKeyDown(KeyCode.C) &&
            canUseInvisibility &&
            !isInvisible &&
            currentEnergy > 0f)
        {
            StartCoroutine(BecomeInvisible());
        }

        if (sliderUI != null)
            sliderUI.SetValue(currentEnergy);
    }

    IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        canUseInvisibility = false;

        foreach (Renderer r in renderers)
            r.material = outlineMaterial;

        float timer = invisDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            currentEnergy =
                (timer / invisDuration) * maxEnergy;

            yield return null;
        }

        foreach (Renderer r in renderers)
            r.material = normalMaterial;

        isInvisible = false;

        currentEnergy = 0f;

        float cooldownTimer = 0f;

        while (cooldownTimer < cooldown)
        {
            cooldownTimer += Time.deltaTime;

            currentEnergy =
                (cooldownTimer / cooldown) * maxEnergy;

            yield return null;
        }

        currentEnergy = maxEnergy;

        canUseInvisibility = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = true;
            rb.linearDamping = 2f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isInWater = false;
            rb.linearDamping = 0f;
        }
    }

    public void OnFormActivated()
    {
        enabled = true;

        if (sliderUI != null)
            sliderUI.Show();
    }

    public void OnFormDeactivated()
    {
        foreach (Renderer r in renderers)
            r.material = normalMaterial;

        isInvisible = false;

        if (sliderUI != null)
            sliderUI.Hide();

        enabled = false;
    }

    public bool IsInvisible => isInvisible;
}