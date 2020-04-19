using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] CanvasGroup menuGroup = default;
    [SerializeField] CanvasGroup screenUI = default;
    [SerializeField] PlayerInputHandler inputHandler = default;
    [SerializeField] AudioSource audioSource = default;

    bool m_isPaused = false;

    public bool canPause = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_isPaused)
            {
                ResumeGame();
            }
            else
            {
                if (canPause)
                {
                    PauseGame();
                }
            }

            audioSource.Play();
        }
    }

    void PauseGame()
    {
        m_isPaused = true;
        Time.timeScale = 0f;

        screenUI.alpha = 0f;
        menuGroup.alpha = 1f;
        menuGroup.blocksRaycasts = true;
        menuGroup.interactable = true;

        inputHandler.SetMenuInteractivity(true);
    }

    public void ResumeGame()
    {
        m_isPaused = false;
        Time.timeScale = 1f;

        screenUI.alpha = 1f;
        menuGroup.alpha = 0f;
        menuGroup.blocksRaycasts = false;
        menuGroup.interactable = false;

        inputHandler.SetMenuInteractivity(false);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
