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
        Debug.Log("Button pressed: " + number);
        enteredCode += number;
        displayText.text = enteredCode;
    }

    public void ClearCode()
    {
        Debug.Log("Clear button pressed");
        enteredCode = "";
        displayText.text = "";
    }

    public void CheckCode()
    {
        Debug.Log("Checking code: " + enteredCode);
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
