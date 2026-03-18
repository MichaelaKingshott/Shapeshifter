using UnityEngine;

public class FanSwitchButton : MonoBehaviour
{
    public FanSpin fanA;
    public FanSpin fanB;

    private bool state = false;
    private bool playerInRange = false;

    void Start()
    {
        fanA.SetFanState(true);
        fanB.SetFanState(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            PressButton();
        }
    }

    public void PressButton()
    {
        state = !state;

        fanA.SetFanState(!state);
        fanB.SetFanState(state);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
