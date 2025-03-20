using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public InventoryItemData data { get; private set; }
    public int stackSize { get; private set; }

    public InventoryItem(InventoryItemData source)
    {
        data = source;
        stackSize = 1; // Default stack size
    }

    public void AddToStack()
    {
        stackSize++;
        Debug.Log($"Stack size increased: {data.displayName} now has {stackSize}");
    }

    public void RemoveFromStack()
    {
        if (stackSize > 0) stackSize--;
    }
}