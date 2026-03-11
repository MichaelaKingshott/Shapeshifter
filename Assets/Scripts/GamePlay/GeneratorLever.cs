using UnityEngine;

public class GeneratorLever : MonoBehaviour
{
    private bool used = false;
    public Transform leverHandle;
    public Vector3 pulledRotation = new Vector3(-60,0,0);

    public void Activate()
    {
        if (used) return;

        used = true;

        leverHandle.localEulerAngles = pulledRotation;

        PowerSystem.instance.SetPower(true);
    }
}
