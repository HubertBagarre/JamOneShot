using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINavigation : MonoBehaviour
{
    public void GoToLobby()
    {
        SceneManager.LoadScene(2);
    }

    public void GoToCredits()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GoToBrazil()
    {
        Application.Quit();
    }
}