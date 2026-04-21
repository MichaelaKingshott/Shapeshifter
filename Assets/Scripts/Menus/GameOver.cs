using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] MonoBehaviour cameraScript;
    [SerializeField] MonoBehaviour pauseScript;

    private ShapeshifterController player;

    void Start()
    {
        player = FindFirstObjectByType<ShapeshifterController>();
    }

    public void Caught()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        cameraScript.enabled = false;

        if (pauseScript != null)
            pauseScript.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        gameOver.SetActive(false);

        Time.timeScale = 1f;

        StartCoroutine(RespawnNextFrame());
    }

    private IEnumerator RespawnNextFrame()
    {
        yield return null; // wait 1 frame

        if (player != null)
        {
            player.RespawnAtCheckpoint();
        }

        cameraScript.enabled = true;

        if (pauseScript != null)
            pauseScript.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}