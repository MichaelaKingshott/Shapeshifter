using UnityEngine;
using System.Collections.Generic;

public class PlayerInventory : MonoBehaviour
{
    public List<KeycardType> keycards = new List<KeycardType>();

    void Awake()
    {
        keycards.Clear();
    }

    public bool HasKeycard(KeycardType type)
    {
        return keycards.Contains(type);
    }

    public void AddKeycard(KeycardType type)
    {
        if (!keycards.Contains(type))
        {
            keycards.Add(type);
        }
    }

    public void RemoveKeycard(KeycardType type)
    {
        if (keycards.Contains(type))
        {
            keycards.Remove(type);
        }
    }
}
