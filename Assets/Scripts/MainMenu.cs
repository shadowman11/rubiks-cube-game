using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayTimed()
    {
        ModeIndicator.isTimed = true;
        PauseMenu.isPaused = false;
        SceneManager.LoadScene("Game");
    }

    public void PlayPractice()
    {
        ModeIndicator.isTimed = false;
        PauseMenu.isPaused = false;
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
