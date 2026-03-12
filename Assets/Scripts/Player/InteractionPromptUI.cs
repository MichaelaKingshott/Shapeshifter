using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance;

    public GameObject promptPanel;
    public TextMeshProUGUI promptText;

    void Awake()
    {
        Instance = this;
        HidePrompt();
    }

    public void ShowPrompt(string message)
    {
        promptPanel.SetActive(true);
        promptText.text = message;
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
    }
}
