using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UINavigation : MonoBehaviour
{
    private SoundManager sm;

    private void Start()
    {
        sm = SoundManager.instance;
    }

    public void GoToLobby()
    {
        sm.PlaySound(5);
        SceneManager.LoadScene(2);
    }

    public void GoToCredits()
    {
        sm.PlaySound(5);
        SceneManager.LoadScene(1);
    }

    public void GoToMenu()
    {
        sm.PlaySound(5);
        SceneManager.LoadScene(0);
    }

    public void GoToBrazil()
    {
        sm.PlaySound(5);
        Application.Quit();
    }
}