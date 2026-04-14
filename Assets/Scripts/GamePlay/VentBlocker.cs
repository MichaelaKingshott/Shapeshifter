using UnityEngine;
using System.Collections;

public class VentBlocker : MonoBehaviour
{
    [Header("Slats")]
    public Transform[] slats;

    [Header("Rotation Settings")]
    public Vector3 closedRotation = new Vector3(0, 0, 0);
    public Vector3 openRotation = new Vector3(-75, 0, 0);

    [Header("Animation")]
    public float speed = 3f;
    public float delayBetweenSlats = 0.1f;

    private bool isOpen = false;
    private Coroutine currentAnimation;

    void Start()
    {
        ApplyInstantState(false);
    }

    public void SetOpen(bool state)
    {
        if (isOpen == state) return;

        isOpen = state;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);

        currentAnimation = StartCoroutine(AnimateSlats());
    }

    IEnumerator AnimateSlats()
    {
        foreach (Transform slat in slats)
        {
            yield return RotateSlat(slat);
            yield return new WaitForSeconds(delayBetweenSlats);
        }
    }

    IEnumerator RotateSlat(Transform slat)
    {
        Quaternion startRot = slat.localRotation;
        Quaternion targetRot = Quaternion.Euler(isOpen ? openRotation : closedRotation);

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;
            t = Mathf.Clamp01(t);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            slat.localRotation = Quaternion.Lerp(startRot, targetRot, smoothT);

            yield return null;
        }

        slat.localRotation = targetRot; // ensure exact final rotation
    }

    void ApplyInstantState(bool state)
    {
        foreach (Transform slat in slats)
        {
            slat.localRotation = Quaternion.Euler(state ? openRotation : closedRotation);
        }
    }
}
