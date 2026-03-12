using UnityEngine;
using System.Collections.Generic;

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
    private AnimalForm currentForm = (AnimalForm)(-1);

    private IAnimalForm currentAbilityForm;

    [SerializeField]
    private AnimalForm startingForm = AnimalForm.Mouse;

    private HashSet<AnimalForm> unlockedForms = new HashSet<AnimalForm>();


    void Start()
    {
        // Unlock starting animal
        unlockedForms.Add(startingForm);

        SwapForm(startingForm);
    }


    void Update()
    {
        // Follow the current animal
        if (currentAnimalInstance != null)
        {
            transform.position = currentAnimalInstance.transform.position;
            transform.rotation = currentAnimalInstance.transform.rotation;
        }

        // Input for swapping forms
        if (Input.GetKeyDown(KeyCode.Alpha1)) TrySwapForm(AnimalForm.Mouse);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TrySwapForm(AnimalForm.Monkey);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TrySwapForm(AnimalForm.Chameleon);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TrySwapForm(AnimalForm.Snake);
        if (Input.GetKeyDown(KeyCode.Alpha5)) TrySwapForm(AnimalForm.Squid);
    }


    void TrySwapForm(AnimalForm form)
    {
        if (!unlockedForms.Contains(form))
        {
            Debug.Log("Form locked: " + form);
            return;
        }

        SwapForm(form);
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

        // IMPORTANT FIX: parent to ShapeShifter
        currentAnimalInstance = Instantiate(prefabToSpawn, spawnPos, spawnRot, transform);

        foreach (var ability in currentAnimalInstance.GetComponents<IAnimalAbility>())
            ability.OnFormActivated();

        currentAbilityForm = currentAnimalInstance.GetComponent<IAnimalForm>();

        currentForm = newForm;

        CameraController cam = FindFirstObjectByType<CameraController>();

        if (cam != null)
        {
            cam.target = currentAnimalInstance.transform;

            AnimalCameraSettings settings = currentAnimalInstance.GetComponent<AnimalCameraSettings>();

            if (settings != null)
                cam.ApplyAnimalCameraSettings(settings);
        }

        Debug.Log("Switched to form: " + currentForm);
    }


    public void UnlockForm(AnimalForm form)
    {
        if (unlockedForms.Contains(form))
            return;

        unlockedForms.Add(form);

        Debug.Log("Unlocked form: " + form);
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

