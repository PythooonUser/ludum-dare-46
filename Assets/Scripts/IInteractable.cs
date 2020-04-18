using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    string GetName();
    string GetAction(InteractionController interactionController);
    void Interact(InteractionController interactionController);
}
