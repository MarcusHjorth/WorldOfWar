
//Provide useful collections and LINQ functions.
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;  //Singleton


    public event System.Action<List<InventoryItem>> onInventoryChanged;

    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    public GameObject[] itemPrefabs; 

    private void Awake()
    {
        // method makes sure there’s only one inventory system, keeps it active across scenes, and sets up the structures to store items
        if (current == null)
        {
            current = this; 
        }
        else if (current != this)
        {
            Destroy(gameObject); 
        }

        DontDestroyOnLoad(gameObject);  // Invenotrysystem is persisted  across scene changes

        inventory = new List<InventoryItem>();  // Player inventory list, remake it into a Dictionary.
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();  // The Dictionary allows you to quickly look up an item by its data (the key)
    }

    public bool IsItemInInventory(string itemId)
    {
        foreach (var item in inventory)
        {
            if (item.data.id == itemId)
            {
                return true; 
            }
        }
        return false; 
    }

    public void Add(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))   // Checks if the item is already in the inventory
        {
            value.AddToStack();     // add to stack if in inventory
        }
        else
        {
            // if not makes a new InventoryItem 
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
            
        }

        
        onInventoryChanged?.Invoke(inventory); // Event to notify other systems (UI) that the inventory has changed.
    }


    
    public void Remove(InventoryItemData referenceData)
    {   
        //Removes the item from the stack or bag (if stack is 0)
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }

        
        onInventoryChanged?.Invoke(inventory);
    }

  /*
    public void SpawnItems()
    {
        foreach (var itemPrefab in itemPrefabs)
        {
            
            InventoryItemData itemData = itemPrefab.GetComponent<ItemObject>().referenceItem;

            
            if (IsItemInInventory(itemData.id))
            {
               
                continue; 
            }

           
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }
    */
}
