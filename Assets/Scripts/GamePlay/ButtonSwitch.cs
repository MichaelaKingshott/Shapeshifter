using UnityEngine;

public class ButtonSwitch : MonoBehaviour, IPressable
{
    public MovingObject targetObject;
    public bool toggle = true;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("InvisiblePlayer"))
        {
            Press();
        }
    }

    public void Press()
    {
        if (targetObject == null) return;

        if (toggle)
        {
            activated = !activated;
            targetObject.SetActiveState(activated);
        }
        else
        {
            targetObject.SetActiveState(true);
        }

        Debug.Log("Button pressed!");
    }
}
