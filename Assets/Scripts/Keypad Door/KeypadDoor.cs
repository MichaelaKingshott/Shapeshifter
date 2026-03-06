using UnityEngine;
using TMPro;

public class KeypadDoor : MonoBehaviour
{
    public string correctCode = "1234";
    private string enteredCode = "";

    public TextMeshProUGUI displayText;
    public SlidingDoor door;
    public KeypadInteract keypadInteract;

    public void AddNumber(string number)
    {
        enteredCode += number;
        displayText.text = enteredCode;
    }

    public void ClearCode()
    {
        enteredCode = "";
        displayText.text = "";
    }

    public void CheckCode()
    {
        if (enteredCode == correctCode)
        {
            OpenDoor();
        }
        else
        {
            displayText.text = "Wrong";
            enteredCode = "";
        }
    }

    void OpenDoor()
    {
        door.OpenDoor();
        keypadInteract.CloseKeypad();
    }
}
