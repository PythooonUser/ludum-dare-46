using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] CanvasGroup gameLostGroup = default;
    [SerializeField] CanvasGroup gameWonGroup = default;
    [SerializeField] CanvasGroup screenUI = default;
    [SerializeField] PlayerInputHandler inputHandler = default;
    [SerializeField] PlayerCharacterController characterController = default;
    [SerializeField] PauseMenuController pauseMenuController = default;
    [SerializeField] Transform speechBubbleAnchor = default;
    [SerializeField] Camera playerCamera = default;
    [SerializeField] Slime slime = default;

    public void GameLostInit()
    {
        pauseMenuController.canPause = false;
    }

    public void GameLost()
    {
        inputHandler.SetMenuInteractivity(true, false);
        characterController.canMove = false;

        StartCoroutine(FadeOutGameLost());
    }

    public void GameWon()
    {
        inputHandler.SetMenuInteractivity(true, false);
        pauseMenuController.canPause = false;
        characterController.canMove = false;

        // Look at slime
        StartCoroutine(LookAtSlime());
        // Slime moves towards you
    }

    IEnumerator FadeOutGameLost()
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

    IEnumerator LookAtSlime()
    {
        yield return new WaitForSeconds(1f);

        float startTime = Time.time;

        while (Time.time < startTime + 4f)
        {
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, Quaternion.LookRotation((speechBubbleAnchor.transform.position - playerCamera.transform.position).normalized, Vector3.up), Time.deltaTime * 0.5f);
            yield return null;
        }

        StartCoroutine(MoveSlimeTowardsPlayer());
    }

    IEnumerator MoveSlimeTowardsPlayer()
    {
        Vector3 moveDirection = (characterController.transform.position - slime.transform.position).normalized;
        slime.DisableCollider();

        while (Vector3.Distance(slime.transform.position, characterController.transform.position) > (slime.size))
        {
            slime.transform.Translate(moveDirection * Time.deltaTime * 4f);
            yield return null;
        }

        slime.PlayAudio(slime.slimeEatingPlayer[Random.Range(0, slime.slimeEatingPlayer.Length)], true);

        StartCoroutine(FadeOutGameWon());
    }

    IEnumerator FadeOutGameWon()
    {
        screenUI.alpha = 0f;
        float startTime = Time.time;

        float percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / 0.5f;
            gameWonGroup.alpha = percent;
            yield return null;
        }

        gameWonGroup.alpha = 1f;

        yield return new WaitForSeconds(4f);

        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
