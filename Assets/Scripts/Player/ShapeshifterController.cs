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

    private HashSet<AnimalForm> unlockedForms => GameManager.Instance.unlockedForms;

    void Start()
    {
        ApplyPersistentData();
    }

    public void ApplyPersistentData()
    {
        // Ensure at least starting form
        if (unlockedForms.Count == 0)
        {
            unlockedForms.Add(startingForm);
        }

        // Move to checkpoint
        if (GameManager.Instance.hasCheckpoint)
        {
            transform.position = GameManager.Instance.checkpointPosition;
        }

        // Restore form
        SwapForm(GameManager.Instance.currentForm);
    }

    void Update()
    {
        foreach (AnimalForm form in System.Enum.GetValues(typeof(AnimalForm)))
        {
            if (Input.GetKeyDown(GetKeyForForm(form)))
            {
                TrySwapForm(form);
            }
        }
    }

    public void TrySwapForm(AnimalForm form)
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

        currentAnimalInstance = Instantiate(prefabToSpawn, spawnPos, spawnRot, transform);

        foreach (var ability in currentAnimalInstance.GetComponents<IAnimalAbility>())
            ability.OnFormActivated();

        currentAbilityForm = currentAnimalInstance.GetComponent<IAnimalForm>();
        currentForm = newForm;

        // SAVE CURRENT FORM
        GameManager.Instance.currentForm = newForm;

        CameraController cam = FindFirstObjectByType<CameraController>();

        if (cam != null)
        {
            cam.target = currentAnimalInstance.transform;
            AnimalCameraSettings settings = currentAnimalInstance.GetComponent<AnimalCameraSettings>();

            if (settings != null)
                cam.ApplyAnimalCameraSettings(settings);
        }
    }

    public void UnlockForm(AnimalForm form)
    {
        if (unlockedForms.Contains(form))
            return;

        unlockedForms.Add(form);
        Debug.Log("Unlocked form: " + form);
    }

    public KeyCode GetKeyForForm(AnimalForm form)
    {
        switch (form)
        {
            case AnimalForm.Mouse: return KeyCode.Alpha1;
            case AnimalForm.Monkey: return KeyCode.Alpha2;
            case AnimalForm.Chameleon: return KeyCode.Alpha3;
            case AnimalForm.Snake: return KeyCode.Alpha4;
            case AnimalForm.Squid: return KeyCode.Alpha5;
            default: return KeyCode.None;
        }
    }

    public bool IsFormUnlocked(AnimalForm form)
    {
        return unlockedForms.Contains(form);
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
