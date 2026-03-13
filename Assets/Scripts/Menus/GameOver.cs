using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    [SerializeField] MonoBehaviour cameraScript;
    [SerializeField] MonoBehaviour pauseScript;

    public void Caught()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f;

        cameraScript.enabled = false;

        if (pauseScript != null)
            pauseScript.enabled = false;

        // Show mouse cursor
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
