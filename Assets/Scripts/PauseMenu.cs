using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool m_GameIsPaused = false;

    [SerializeField]
    public GameObject m_PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(m_GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        m_PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        m_GameIsPaused = true;
    }

    public void Resume()
    {
        m_PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        m_GameIsPaused = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        m_GameIsPaused = false;
        SceneManager.LoadScene(1);
    }
    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
