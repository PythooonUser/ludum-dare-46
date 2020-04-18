using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] new string name = default;

    public string GetName()
    {
        return name;
    }

    public void PickUp()
    {
        Destroy(gameObject);
    }
}
