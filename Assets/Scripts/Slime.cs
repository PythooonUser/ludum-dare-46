using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour, IInteractable
{
    [Header("Animation")]
    [SerializeField] float speed = 0.25f;
    [SerializeField] float size = 1f;

    [Header("References")]
    [SerializeField] Transform player = default;

    void Update()
    {
        HandleIdleAnimation();
    }

    void HandleIdleAnimation()
    {
        float percent = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
        Vector3 scale = Vector3.one * size * Mathf.Lerp(0.995f, 1.05f, percent);

        transform.localScale = scale;
    }

    void GrowSize()
    {
        size += 0.15f;
    }

    void ShrinkSize()
    {
        size -= 0.15f;
    }

    public string GetName()
    {
        return "Slime";
    }

    public string GetAction(InteractionController interactionController)
    {
        return interactionController.isHoldingResource ? "to feed" : "";
    }

    public void Interact(InteractionController interactionController)
    {
        if (!interactionController.isHoldingResource) { return; }

        HandHeld handHeld = interactionController.GetHandHeld();
        interactionController.SetHandHeld(null, false);

        Resource worldModelInstance = Instantiate(handHeld.GetWorldModel(), transform.position + Vector3.up * size * 0.5f, Random.rotation, transform);
        worldModelInstance.transform.localScale = Vector3.one * size * 0.5f;

        GrowSize();

        interactionController.RefreshSelection();
    }
}
