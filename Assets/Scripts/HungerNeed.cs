using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HungerNeed
{
    public ResourceType type;
    public int amount;

    public void Consume()
    {
        amount -= 1;
    }
}
