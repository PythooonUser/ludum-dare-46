using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    [SerializeField] new string name = default;
    [SerializeField] GameObject handheldPrefab = default;

    public string GetName()
    {
        return name;
    }

    public GameObject PickUp()
    {
        Destroy(gameObject);
        return handheldPrefab;
    }
}
