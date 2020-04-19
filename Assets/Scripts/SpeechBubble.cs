using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] Transform anchor = default;
    [SerializeField] Transform player = default;

    [SerializeField] Sprite[] sprites = default;

    [Header("References")]
    [SerializeField] Image contentImage = default;
    [SerializeField] Text playerText = default;

    void Start()
    {
        contentImage.gameObject.SetActive(true);
        playerText.gameObject.SetActive(false);
    }

    void Update()
    {
        FacePlayer();
    }

    void FacePlayer()
    {
        transform.position = anchor.position;

        transform.rotation = Quaternion.LookRotation(
            Vector3.ProjectOnPlane(transform.position - player.position, Vector3.up),
            Vector3.up
        );
    }

    public void UpdateContent(ResourceType type)
    {
        if (type == ResourceType.Player)
        {
            contentImage.gameObject.SetActive(false);
            playerText.gameObject.SetActive(true);
        }
        else
        {
            Sprite sprite = sprites[(int)type];
            contentImage.sprite = sprite;
        }
    }
}
