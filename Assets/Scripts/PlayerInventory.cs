using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // All keys the player has collected
    private HashSet<string> collectedKeys = new HashSet<string>();

    public void AddKey(string keyId)
    {
        if (!string.IsNullOrEmpty(keyId))
        {
            collectedKeys.Add(keyId);
            Debug.Log("Picked up key: " + keyId);
        }
    }

    public bool HasKey(string keyId)
    {
        return collectedKeys.Contains(keyId);
    }

    public bool HasAllKeys(string[] requiredKeys)
    {
        foreach (var key in requiredKeys)
        {
            if (!collectedKeys.Contains(key))
                return false;
        }
        return true;
    }
}
