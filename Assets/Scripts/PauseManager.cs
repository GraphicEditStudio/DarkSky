using UnityEngine;
using UnityEngine.Audio;

public class PauseManager : MonoBehaviour
{
    public GameObject PausePanel;
    private bool m_isPaused = false;


    void Update()
    {
        PauseToggle();
    }

    public void PauseToggle()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_isPaused) // unPause
            {
                m_isPaused = false;

                PausePanel.SetActive(false);
                Time.timeScale = 1;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

            }
            else // Pause
            {
                m_isPaused = true;

                PausePanel.SetActive(true);
                Time.timeScale = 0;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }

        }

    }

    public void QuitGame()
    {
        Application.Quit();

    }


    public void ResumeGame()
    {
        m_isPaused = false;

        Time.timeScale = 1;
        PausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


}
