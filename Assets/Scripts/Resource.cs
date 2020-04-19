using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour, IInteractable
{
    [SerializeField] new string name = "Resource";
    [SerializeField] HandHeld handHeldPrefab = default;
    public ResourceType type;

    public string GetName()
    {
        return name;
    }

    public string GetAction(InteractionController interactionController)
    {
        return interactionController.isHoldingResource ? "to swap" : "to pick up";
    }

    public void Interact(InteractionController interactionController)
    {
        Destroy(gameObject);
        interactionController.SetHandHeld(handHeldPrefab);
    }
}
