using UnityEngine;
using System.Collections;

public class ChameleonMovement : MonoBehaviour, IAnimalAbility
{
    [Header("Movement")]
    public float moveSpeed = 4f;
    public float turnSpeed = 8f;

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

    [Tooltip("Main physics collider that keeps the chameleon grounded")]
    public Collider groundCollider;

    [Header("Animation")]
    public float moveThreshold = 0.1f; // Minimum speed to trigger walking
    private Animator anim;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        renderers = GetComponentsInChildren<Renderer>();
        colliders = GetComponentsInChildren<Collider>();

        if (groundCollider == null && colliders.Length > 0)
            groundCollider = colliders[0];

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Camera-relative
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 move = camForward * v + camRight * h;

        rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);

        if (move != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
        }

        // Update Animator Speed
        float speed = new Vector3(move.x, 0, move.z).magnitude;
        float horizontalSpeed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;
        anim.SetFloat("Speed", horizontalSpeed < moveThreshold ? 0f : horizontalSpeed);


        // Invisibility
        if (Input.GetKeyDown(KeyCode.C) && canUseInvisibility && !isInvisible)
            StartCoroutine(BecomeInvisible());
    }

    private IEnumerator BecomeInvisible()
    {
        isInvisible = true;
        canUseInvisibility = false;

        foreach (Renderer r in renderers)
            r.material = outlineMaterial;

        foreach (Collider c in colliders)
            if (c != groundCollider) c.enabled = false;

        yield return new WaitForSeconds(invisDuration);

        foreach (Renderer r in renderers)
            r.material = normalMaterial;

        foreach (Collider c in colliders)
            c.enabled = true;

        isInvisible = false;

        yield return new WaitForSeconds(cooldown);
        canUseInvisibility = true;
    }

    public void OnFormActivated() => this.enabled = true;

    public void OnFormDeactivated()
    {
        foreach (Renderer r in renderers) r.material = normalMaterial;
        foreach (Collider c in colliders) c.enabled = true;
        this.enabled = false;
    }
}





