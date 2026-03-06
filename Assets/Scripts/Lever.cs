using UnityEngine;

public class Lever : MonoBehaviour
{
    public MovingObject targetObject;
    private bool isOn = false;

    public void Interact()
    {
        isOn = !isOn;
        targetObject.SetActiveState(isOn);
    }
}
