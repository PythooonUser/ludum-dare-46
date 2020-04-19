using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] CanvasGroup menuGroup = default;
    [SerializeField] CanvasGroup introGroup = default;
    [SerializeField] CanvasGroup keyPressGroup = default;

    [SerializeField] AudioSource introClip = default;

    [SerializeField] bool m_keyPressStartsGame = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        menuGroup.alpha = 0f;
        StartCoroutine(FadeInGroup(menuGroup, 2f, 2f));
    }

    void Update()
    {
        if (!m_keyPressStartsGame) { return; }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_keyPressStartsGame = false;
            if (introClip.isPlaying)
            {
                introClip.Stop();
            }
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
    }

    public void StartGame()
    {
        StartCoroutine(FadeOutGroup(menuGroup, 2f, 0.1f));
        StartCoroutine(FadeInGroup(introGroup, 2f, 2f));

        StartCoroutine(FadeInGroup(keyPressGroup, 1f, 6f + 0f));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator FadeOutGroup(CanvasGroup group, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;

        group.alpha = 1f;
        group.interactable = false;

        float percent = 0f;
        while (percent < 1f)
        {
            percent = Mathf.Clamp01((Time.time - startTime) / duration);
            group.alpha = 1f - percent;
            yield return null;
        }

        group.alpha = 0f;
    }

    IEnumerator FadeInGroup(CanvasGroup group, float duration, float delay)
    {
        yield return new WaitForSeconds(delay);

        float startTime = Time.time;

        group.alpha = 0f;
        group.interactable = false;

        if (group == introGroup)
        {
            introClip.Play();
        }

        float percent = 0f;
        while (percent < 1f)
        {
            percent = Mathf.Clamp01((Time.time - startTime) / duration);
            group.alpha = percent;
            yield return null;
        }

        group.alpha = 1f;
        group.interactable = true;

        if (group == keyPressGroup)
        {
            m_keyPressStartsGame = true;
        }
    }
}
