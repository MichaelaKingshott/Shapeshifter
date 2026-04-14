using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // ✅ THIS IS WHAT YOUR ERROR IS ABOUT — MUST EXIST
    public HashSet<AnimalForm> unlockedForms = new HashSet<AnimalForm>();

    public HashSet<AnimalForm> consumedCorpses = new HashSet<AnimalForm>();

    public Vector3 checkpointPosition;
    public bool hasCheckpoint = false;

    public AnimalForm currentForm = AnimalForm.Mouse;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ShapeshifterController player = FindFirstObjectByType<ShapeshifterController>();

        if (player != null)
        {
            player.ApplyPersistentData();
        }
    }
}
