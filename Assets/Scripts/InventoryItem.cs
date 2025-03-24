using System;
using UnityEngine;

[Serializable]  // Save the class data between game sessions also Inspector
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
       
    }

    public void RemoveFromStack()
    {
        if (stackSize > 0) stackSize--;
    }
}