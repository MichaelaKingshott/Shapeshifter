using UnityEngine;

public class FanSpin : MonoBehaviour
{
    public float speed = 200f;
    public Vector3 rotationAxis = Vector3.forward;
    public bool isOn = true;

    public GameObject blocker; // assign in inspector

    void Update()
    {
        if (isOn)
        {
            transform.Rotate(rotationAxis * speed * Time.deltaTime);
        }
    }

    public void SetFanState(bool state)
    {
        isOn = state;

        if (blocker != null)
        {
            blocker.SetActive(state); // ON = blocks, OFF = open
        }
    }
}