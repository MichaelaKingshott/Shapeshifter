using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelEnd3D : MonoBehaviour
{
    public string endSceneName = "EndScreen";
    public float delayBeforeEnd = 1.5f;

    public CanvasGroup fadeCanvas; // assign in inspector
    public float fadeSpeed = 2f;

    private bool hasEnded = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasEnded) return;

        if (other.CompareTag("Player"))
        {
            hasEnded = true;

            // Freeze player
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // Disable movement script (if you have one)
            MonoBehaviour movement = other.GetComponent<MonoBehaviour>();
            if (movement != null)
                movement.enabled = false;

            StartCoroutine(EndSequence());
        }
    }

    IEnumerator EndSequence()
    {
        float timer = 0f;

        // Fade to black
        while (timer < 1f)
        {
            timer += Time.deltaTime * fadeSpeed;
            fadeCanvas.alpha = timer;
            yield return null;
        }

        yield return new WaitForSeconds(delayBeforeEnd);

        SceneManager.LoadScene(endSceneName);
    }
}