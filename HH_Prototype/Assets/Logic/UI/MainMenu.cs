using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void StartGame(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
