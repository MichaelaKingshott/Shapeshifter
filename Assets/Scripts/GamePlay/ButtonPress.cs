using UnityEngine;

public class ButtonPress : MonoBehaviour
{
    public MovingObject targetObject;
    public bool toggle = true;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(toggle)
            {
                activated = !activated;
                targetObject.SetActiveState(activated);
            }
            else
            {
                targetObject.SetActiveState(true);
            }
        }
    }
}
