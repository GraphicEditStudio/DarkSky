using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject SettingsPanel;
    public GameObject CreditsPanel;
    public GameObject HowsPanel;


    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    void Start()
    {
        SettingsPanel.SetActive(false);
        HowsPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        SettingsPanel.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        SettingsPanel.SetActive(false);
    }

    public void OpenCreditsPanel()
    {
        CreditsPanel.SetActive(true);
    }
    public void CloseCreditsPanel()
    {
        CreditsPanel.SetActive(false);
    }

    public void OpenHowPanel()
    {
        HowsPanel.SetActive(true);
    }
    public void CloseHowPanel()
    {
        HowsPanel.SetActive(false);
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
