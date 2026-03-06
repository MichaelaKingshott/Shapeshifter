using UnityEngine;
using System.Collections;

public class ChameleonMovement : MonoBehaviour, IAnimalAbility
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float turnSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Invisibility Ability")]
    public float invisDuration = 3f;
    public float cooldown = 5f;

    [Header("Outline Materials")]
    public Material normalMaterial;
    public Material outlineMaterial;

    private Renderer[] renderers;
    private Collider[] colliders;
    private Rigidbody rb;
    private bool isInvisible = false;
    private bool canUseInvisibility = true;
    private bool isGrounded = true;
    public Collider groundCollider;

    [Header("Animation")]
    public float moveThreshold = 0.1f;
    private Animator anim;

    [Header("Water")]
    public float sinkForce = 6f;
    private bool isInWater = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();
        if (groundCollider == null && colliders.Length > 0) groundCollider = colliders[0];
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleInvisibility();

        if (isInWater)
        {
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f); // stop horizontal movement
            rb.AddForce(Vector3.down * sinkForce, ForceMode.Acceleration); // sink
        }
    }

    private void HandleMovement()
    {
        if (isInWater) return; // no horizontal movement in water

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 camForward = Camera.main.transform.forward; camForward.y = 0f; camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right; camRight.y = 0f; camRight.Normalize();
        Vector3 move = camForward * v + camRight * h;

        rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        anim.SetFloat("Speed", horizontalSpeed < moveThreshold ? 0f : horizontalSpeed);
    }

    private void HandleJump()
    {
        if (isInWater) return; // cannot jump in water

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            anim.SetTrigger("Jump");
        }

        anim.SetBool("isGrounded", isGrounded);
    }

    private void HandleInvisibility()
    {
        if (Input.GetKeyDown(KeyCode.C) && canUseInvisibility && !isInvisible)
            StartCoroutine(BecomeInvisible());
    }

    private IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        canUseInvisibility = false;

        foreach (Renderer r in renderers) r.material = outlineMaterial;
        foreach (Collider c in colliders) if (c != groundCollider) c.enabled = false;

        yield return new WaitForSeconds(invisDuration);

        foreach (Renderer r in renderers) r.material = normalMaterial;
        foreach (Collider c in colliders) c.enabled = true;

        isInvisible = false;
        yield return new WaitForSeconds(cooldown);
        canUseInvisibility = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
            isInWater = false;
    }

    public void OnFormActivated() => this.enabled = true;
    public void OnFormDeactivated()
    {
        foreach (Renderer r in renderers) r.material = normalMaterial;
        foreach (Collider c in colliders) c.enabled = true;
        this.enabled = false;
    }
}