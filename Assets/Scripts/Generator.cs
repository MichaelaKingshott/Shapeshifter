using UnityEngine;

public class Generator : MonoBehaviour
{
    public MovingObject[] poweredObjects;
    private bool powerOn = false;

    public void ActivateGenerator()
    {
        powerOn = true;

        foreach(MovingObject obj in poweredObjects)
        {
            obj.SetActiveState(true);
        }
    }
}