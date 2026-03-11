using UnityEngine;

public class ShapeshifterController : MonoBehaviour
{
    [System.Serializable]
    public class AnimalFormData
    {
        public AnimalForm form;
        public GameObject prefab;
    }

    public AnimalFormData[] animalPrefabs;

    private GameObject currentAnimalInstance;
    private AnimalForm currentForm;

    private IAnimalForm currentAbilityForm;

    [SerializeField]
    private AnimalForm startingForm = AnimalForm.Mouse;

    void Start()
    {
        SwapForm(startingForm);
    }

    void Update()
    {
        if (currentAnimalInstance != null)
        {
            transform.position = currentAnimalInstance.transform.position;
            transform.rotation = currentAnimalInstance.transform.rotation;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwapForm(AnimalForm.Mouse);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwapForm(AnimalForm.Bird);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwapForm(AnimalForm.Squid);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwapForm(AnimalForm.Chameleon);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SwapForm(AnimalForm.Monkey);
    }

    public void SwapForm(AnimalForm newForm)
    {
        if (newForm == currentForm)
            return;

        Vector3 spawnPos;
        Quaternion spawnRot;

        if (currentAnimalInstance != null)
        {
            spawnPos = currentAnimalInstance.transform.position;
            spawnRot = currentAnimalInstance.transform.rotation;

            foreach (var ability in currentAnimalInstance.GetComponents<IAnimalAbility>())
                ability.OnFormDeactivated();

            Destroy(currentAnimalInstance);
        }
        else
        {
            spawnPos = transform.position;
            spawnRot = transform.rotation;
        }

        GameObject prefabToSpawn = null;

        foreach (var data in animalPrefabs)
        {
            if (data.form == newForm)
            {
                prefabToSpawn = data.prefab;
                break;
            }
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("No prefab found for form: " + newForm);
            return;
        }

        currentAnimalInstance = Instantiate(prefabToSpawn, spawnPos, spawnRot);

        foreach (var ability in currentAnimalInstance.GetComponents<IAnimalAbility>())
            ability.OnFormActivated();

        currentAbilityForm = currentAnimalInstance.GetComponent<IAnimalForm>();

        currentForm = newForm;

        // Update camera target
        CameraController cam = FindFirstObjectByType<CameraController>();
        if (cam != null)
            cam.target = currentAnimalInstance.transform;

        Debug.Log("Switched to form: " + currentForm);
    }

    public AnimalForm GetCurrentForm() => currentForm;

    public GameObject GetCurrentAnimalInstance() => currentAnimalInstance;

    public bool IsInvisible()
    {
        if (currentAbilityForm == null)
            return false;

        return currentAbilityForm.IsInvisible;
    }
}

