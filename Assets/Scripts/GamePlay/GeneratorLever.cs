using UnityEngine;
using TMPro;

public class GeneratorLever : MonoBehaviour
{
    private bool used = false;
    private bool playerInRange = false;

    public Transform leverHandle;
    public Vector3 pulledRotation = new Vector3(-60,0,0);

    public TMP_Text popupText;
    public string generatorMessage = "Press E to start Generator";

    void Update()
    {
        if(playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Activate();
        }
    }

    public void Activate()
    {
        if (used) return;

        used = true;

        leverHandle.localRotation = Quaternion.Euler(pulledRotation);

        PowerSystem.instance.SetPower(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
            popupText.text = generatorMessage;
            popupText.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
            popupText.gameObject.SetActive(false);
        }
    }
}
