using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] Transform anchor = default;
    [SerializeField] Transform player = default;

    void Update()
    {
        transform.position = anchor.position;

        transform.rotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(transform.position - player.position, Vector3.up),
            Vector3.up
        );
    }
}
