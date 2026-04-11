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

    void Start()
    {
        ApplyInstantState(false);
    }

    public void SetOpen(bool state)
    {
        if (isOpen == state) return;

        isOpen = state;
        StopAllCoroutines();
        StartCoroutine(AnimateSlats());
    }

    IEnumerator AnimateSlats()
    {
        foreach (Transform slat in slats)
        {
            StartCoroutine(RotateSlat(slat));
            yield return new WaitForSeconds(delayBetweenSlats);
        }
    }

    IEnumerator RotateSlat(Transform slat)
    {
        Quaternion startRot = slat.localRotation;
        Quaternion targetRot = Quaternion.Euler(isOpen ? openRotation : closedRotation);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * speed;
            slat.localRotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
    }

    void ApplyInstantState(bool state)
    {
        foreach (Transform slat in slats)
        {
            slat.localRotation = Quaternion.Euler(state ? openRotation : closedRotation);
        }
    }
}
