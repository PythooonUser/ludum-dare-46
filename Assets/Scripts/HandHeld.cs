using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHeld : MonoBehaviour
{
    [SerializeField] Resource resource = default;
    public ResourceType type { get { return resource.type; } private set { type = value; } }

    public Resource GetWorldModel()
    {
        return resource;
    }
}
