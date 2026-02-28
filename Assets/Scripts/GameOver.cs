using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
	[SerializeField] GameObject gameOver;

	public void Caught()
    {
        gameOver.SetActive(true);
        Time.timeScale = 0f; // Freeze game
    }

    public void Home()
    {
        Time.timeScale = 1f; // Make sure time resets
        SceneManager.LoadScene("Main Menu");
    }

    public void Restart()
    {
        gameOver.SetActive(false);
        Time.timeScale = 1f; // Make sure time resets
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
