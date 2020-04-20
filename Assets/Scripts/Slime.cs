using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IInteractable
{
    [Header("Idle Animation")]
    [SerializeField] float speed = 0.25f;
    public float size = 1f;
    [SerializeField] Color hurtColor = default;
    [SerializeField] Color healColor = default;
    [SerializeField] Color deathColor = default;
    [SerializeField] float flashTime = 0.25f;

    [Header("Hunger")]
    [SerializeField] bool canBeFed = true;
    [SerializeField] float starveTick = 2f;
    [SerializeField] float sizeChangeAmountByStarving = 0.05f;
    [SerializeField] float sizeChangeAmountByFeeding = 0.15f;
    [SerializeField] float starvationThreshold = 0.5f;
    [SerializeField] ParticleSystem hurtParticles = default;
    [SerializeField] HungerNeed[] hungerNeeds = default;

    [Header("References")]
    [SerializeField] SpeechBubble speechBubble = default;
    [SerializeField] MeshRenderer meshRenderer = default;
    [SerializeField] GameObject colliderParent = default;
    [SerializeField] GameManager gameManager = default;

    int m_currentNeedIndex = 0;
    float m_timeSinceLastStarveTick = 0f;
    bool m_isAlive = true;
    bool m_canStarve = true;
    Color m_initialColor = default;

    void Start()
    {
        UpdateSpeechBubble(hungerNeeds[m_currentNeedIndex].type);
        m_initialColor = meshRenderer.material.color;
    }

    void Update()
    {
        HandleIdleAnimation();
        HandleStarving();
    }

    void HandleIdleAnimation()
    {
        if (!m_isAlive) { return; }

        float percent = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        Vector3 scale = Vector3.one * size * Mathf.Lerp(0.995f, 1.05f, percent);

        transform.localScale = scale;
    }

    void GrowSize()
    {
        size += sizeChangeAmountByFeeding;
        AdjustParticleSystem();
    }

    void ShrinkSize()
    {
        size -= sizeChangeAmountByFeeding;
        AdjustParticleSystem();
    }

    void AdjustParticleSystem()
    {
        var shape = hurtParticles.shape;
        shape.radius = size;
    }

    public string GetName()
    {
        return m_isAlive ? "Slime" : "Dead Slime";
    }

    public string GetAction(InteractionController interactionController)
    {
        return (interactionController.isHoldingResource && canBeFed && m_isAlive) ? "to feed" : "";
    }

    public void Interact(InteractionController interactionController)
    {
        if (!interactionController.isHoldingResource || !canBeFed || !m_isAlive) { return; }

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
            GrowSize();
            StartCoroutine(HealFlash());
            m_timeSinceLastStarveTick = 0f;

            if (hungerNeeds[m_currentNeedIndex].amount <= 0)
            {
                m_currentNeedIndex += 1;

                if (m_currentNeedIndex > hungerNeeds.Length - 1)
                {
                    canBeFed = false;
                    m_canStarve = false;
                    UpdateSpeechBubble(ResourceType.Player);
                    gameManager.GameWon();

                }
                else
                {
                    UpdateSpeechBubble(hungerNeeds[m_currentNeedIndex].type);
                }
            }
        }
        else
        {
            ShrinkSize();
            StartCoroutine(HurtFlash());
            CheckIsAlive();
        }
    }

    void UpdateSpeechBubble(ResourceType type)
    {
        speechBubble.UpdateContent(type);
    }

    void HandleStarving()
    {
        if (!m_isAlive || !m_canStarve) { return; }

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
        StartCoroutine(HurtFlash());
        CheckIsAlive();
    }

    IEnumerator HurtFlash()
    {
        Color color = m_initialColor;
        float startTime = Time.time;

        hurtParticles.Play();

        float percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / flashTime;
            Color flashColor = Color.Lerp(color, hurtColor, percent);
            meshRenderer.material.SetColor("_Color", flashColor);
            yield return null;
        }

        startTime = Time.time;
        percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / flashTime;
            Color flashColor = Color.Lerp(hurtColor, color, percent);
            meshRenderer.material.SetColor("_Color", flashColor);
            yield return null;
        }

        meshRenderer.material.SetColor("_Color", color);
    }

    IEnumerator HealFlash()
    {
        Color color = m_initialColor;
        float startTime = Time.time;

        float percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / flashTime;
            Color flashColor = Color.Lerp(color, healColor, percent);
            meshRenderer.material.SetColor("_Color", flashColor);
            yield return null;
        }

        startTime = Time.time;
        percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / flashTime;
            Color flashColor = Color.Lerp(healColor, color, percent);
            meshRenderer.material.SetColor("_Color", flashColor);
            yield return null;
        }

        meshRenderer.material.SetColor("_Color", color);
    }

    IEnumerator DieAnimation()
    {
        Color color = meshRenderer.material.color;
        Vector3 scale = transform.localScale;
        Vector3 deathScale = new Vector3(scale.x * 1.25f, 0.1f, scale.z * 1.25f);

        float startTime = Time.time;

        float percent = 0f;
        while (percent < 1f)
        {
            percent = (Time.time - startTime) / 3f;

            Color flashColor = Color.Lerp(color, deathColor, percent);
            meshRenderer.material.SetColor("_Color", flashColor);

            Vector3 flashScale = Vector3.Lerp(scale, deathScale, percent);
            transform.localScale = flashScale;

            yield return null;
        }

        meshRenderer.material.SetColor("_Color", deathColor);
        transform.localScale = deathScale;

        yield return new WaitForSeconds(1f);
        gameManager.GameLost();
    }

    void CheckIsAlive()
    {
        if (size < starvationThreshold)
        {
            m_isAlive = false;
            speechBubble.gameObject.SetActive(false);
            StopAllCoroutines();
            StartCoroutine(FlashCollider());
            StartCoroutine(DieAnimation());
        }
    }

    IEnumerator FlashCollider()
    {
        colliderParent.SetActive(false);
        yield return null;
        colliderParent.SetActive(true);
    }

    public void DisableCollider()
    {
        colliderParent.SetActive(false);
    }
}
