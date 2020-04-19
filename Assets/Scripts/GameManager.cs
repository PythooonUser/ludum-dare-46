using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameLostGroup = default;
    [SerializeField] CanvasGroup screenUI = default;
    [SerializeField] PlayerInputHandler inputHandler = default;
    [SerializeField] PauseMenuController pauseMenuController = default;

    public void GameLost()
    {
        inputHandler.SetMenuInteractivity(true, false);
        pauseMenuController.canPause = false;

        StartCoroutine(FadeOut());
    }

    public void GameWon() { }

    IEnumerator FadeOut()
    {
        screenUI.alpha = 0f;
        float startTime = Time.time;

        float percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / 3f;
            gameLostGroup.alpha = percent;
            yield return null;
        }

        gameLostGroup.alpha = 1f;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
