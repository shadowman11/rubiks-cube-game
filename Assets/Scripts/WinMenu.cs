using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{

    public GameObject winMenu;
    public static bool hasWon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winMenu.SetActive(false);
        hasWon = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO
        // read cube state
        // if in original state
        // hasWon = true
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
