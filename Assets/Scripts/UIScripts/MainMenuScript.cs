using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public AudioClip mainMenuTheme;

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void QuitGame()
    {
        Debug.Log("Game Stopped");
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        AudioManager.Instance.PlayClip(mainMenuTheme);
        SceneManager.LoadScene("MainMenu");
    }
}
