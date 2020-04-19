using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IInteractable
{
    [Header("Animation")]
    [SerializeField] float speed = 0.25f;
    [SerializeField] float size = 1f;

    [Header("Hunger")]
    [SerializeField] bool canBeFed = true;
    [SerializeField] float starveTick = 2f;
    [SerializeField] float sizeChangeAmountByStarving = 0.05f;
    [SerializeField] float sizeChangeAmountByFeeding = 0.15f;
    [SerializeField] HungerNeed[] hungerNeeds = default;

    [Header("References")]
    [SerializeField] Transform player = default;
    [SerializeField] SpeechBubble speechBubble = default;

    int m_currentNeedIndex = 0;
    float m_timeSinceLastStarveTick = 0f;

    void Start()
    {
        UpdateSpeechBubble(hungerNeeds[m_currentNeedIndex].type);
    }

    void Update()
    {
        HandleIdleAnimation();
        HandleStarving();
    }

    void HandleIdleAnimation()
    {
        float percent = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        Vector3 scale = Vector3.one * size * Mathf.Lerp(0.995f, 1.05f, percent);

        transform.localScale = scale;
    }

    void GrowSize()
    {
        size += sizeChangeAmountByFeeding;
    }

    void ShrinkSize()
    {
        size -= sizeChangeAmountByFeeding;
    }

    public string GetName()
    {
        return "Slime";
    }

    public string GetAction(InteractionController interactionController)
    {
        return (interactionController.isHoldingResource && canBeFed) ? "to feed" : "";
    }

    public void Interact(InteractionController interactionController)
    {
        if (!interactionController.isHoldingResource || !canBeFed) { return; }

        HandHeld handHeld = interactionController.GetHandHeld();
        interactionController.SetHandHeld(null, false);

        Resource worldModelInstance = Instantiate(handHeld.GetWorldModel(), transform.position + Vector3.up * size * 0.5f, Random.rotation, transform);
        worldModelInstance.transform.localScale = Vector3.one * size * 0.5f;

        Consume(handHeld.type);

        interactionController.RefreshSelection();
    }

    void Consume(ResourceType type)
    {
        if (type == hungerNeeds[m_currentNeedIndex].type)
        {
            hungerNeeds[m_currentNeedIndex].Consume();
            // TODO: Let the slime grow and show next item in speech bubble
            GrowSize();
            m_timeSinceLastStarveTick = 0f;

            if (hungerNeeds[m_currentNeedIndex].amount <= 0)
            {
                m_currentNeedIndex += 1;

                if (m_currentNeedIndex > hungerNeeds.Length - 1)
                {
                    // TODO: Play endgame cinematic
                    Debug.Log("END");
                    canBeFed = false;
                    UpdateSpeechBubble(ResourceType.Player);
                }
                else
                {
                    UpdateSpeechBubble(hungerNeeds[m_currentNeedIndex].type);
                }
            }
        }
        else
        {
            // TODO: Let the slime shrink
            ShrinkSize();
        }
    }

    void UpdateSpeechBubble(ResourceType type)
    {
        speechBubble.UpdateContent(type);
    }

    void HandleStarving()
    {
        m_timeSinceLastStarveTick += Time.deltaTime;

        if (m_timeSinceLastStarveTick >= starveTick)
        {
            Starve();
            m_timeSinceLastStarveTick = 0f;
        }
    }

    void Starve()
    {
        size -= sizeChangeAmountByStarving;
    }
}
