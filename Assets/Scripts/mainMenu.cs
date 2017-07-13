using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class mainMenu : MonoBehaviour {

    public string soloModeString;
    public string versusModeString;

    public void PlaySolo()
    {
        SceneManager.LoadScene(soloModeString);
        Time.timeScale = 1;
    }

    public void PlayVersus()
    {
        SceneManager.LoadScene(versusModeString);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
