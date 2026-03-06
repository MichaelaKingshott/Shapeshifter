using UnityEngine;
using TMPro;
using System.Collections;

public class DoorMessageUI : MonoBehaviour
{
    public TextMeshProUGUI messageText;

    private Coroutine messageCoroutine;

    void Start()
    {
        messageText.text = "";
    }

    public void ShowMessage(string message, float duration = 2f)
    {
        if (messageCoroutine != null)
        {
            StopCoroutine(messageCoroutine);
        }

        messageCoroutine = StartCoroutine(DisplayMessage(message, duration));
    }

    IEnumerator DisplayMessage(string message, float duration)
    {
        messageText.text = message;

        yield return new WaitForSeconds(duration);

        messageText.text = "";
        messageCoroutine = null;
    }
}
