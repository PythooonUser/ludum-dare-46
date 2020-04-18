using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandHeld : MonoBehaviour
{
    [SerializeField] Resource resource = default;

    public Resource GetWorldModel()
    {
        return resource;
    }
}
