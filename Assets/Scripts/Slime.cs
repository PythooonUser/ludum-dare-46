using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] float speed = 0.25f;
    [SerializeField] float size = 1f;

    [Header("References")]
    [SerializeField] Transform player = default;
    [SerializeField] Transform speechBubbleAnchor = default;

    void Update()
    {
        HandleIdleAnimation();
        // HandleSizeChange();
        HandleSpeechBubbleRotation();
    }

    void HandleIdleAnimation()
    {
        float percent = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        Vector3 scale = Vector3.one * size * Mathf.Lerp(0.995f, 1.05f, percent);

        transform.localScale = scale;
    }

    void HandleSizeChange()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            size -= 0.15f;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            size += 0.15f;
        }
    }

    void HandleSpeechBubbleRotation()
    {
        speechBubbleAnchor.LookAt(-player.position);
    }
}
