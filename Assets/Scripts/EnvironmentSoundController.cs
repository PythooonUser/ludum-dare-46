using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentSoundController : MonoBehaviour
{
    [SerializeField] AudioClip[] clips = default;
    [SerializeField] Vector2 timeBetweenClips = new Vector2(2f, 5f);
    [SerializeField] AudioSource audioSource = default;

    float m_timeSinceLastClip = 0f;
    float m_timeUntilNextClip = 0f;

    void Start()
    {
        m_timeUntilNextClip = Random.Range(timeBetweenClips.x, timeBetweenClips.y);
    }

    void Update()
    {
        if (m_timeSinceLastClip >= m_timeUntilNextClip)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.clip = clip;
            audioSource.Play();

            m_timeSinceLastClip = 0f;
            m_timeUntilNextClip = clip.length + Random.Range(timeBetweenClips.x, timeBetweenClips.y);
        }
        else
        {
            m_timeSinceLastClip += Time.deltaTime;
        }
    }
}
