using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public static InventorySystem current;
    public event System.Action<List<InventoryItem>> onInventoryChanged;

    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory { get; private set; }

    public GameObject[] itemPrefabs; // The item prefabs to spawn

    private void Awake()
    {
        if (current == null)
        {
            current = this; // Set this as the singleton instance
        }
        else if (current != this)
        {
            Destroy(gameObject); // Destroy any duplicate InventorySystem if exists
        }

        DontDestroyOnLoad(gameObject);

        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public bool IsItemInInventory(string itemId)
    {
        foreach (var item in inventory)
        {
            if (item.data.id == itemId)
            {
                return true; // Item is already in inventory
            }
        }
        return false; // Item not found in inventory
    }

    public void Add(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.AddToStack(); // Stack the item
            Debug.Log($"Stack size increased: {referenceData.displayName} now has {value.stackSize}");
        }
        else
        {
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
            Debug.Log($"Added new item: {referenceData.displayName}, Stack size: {newItem.stackSize}");
        }

        // Trigger UI update
        onInventoryChanged?.Invoke(inventory); // Notify UI to refresh
    }


    
    public void Remove(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            value.RemoveFromStack();
            if (value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }

        // Notify UI update immediately
        onInventoryChanged?.Invoke(inventory);
    }

    // Method to spawn items (only if they are not in the inventory)
    public void SpawnItems()
    {
        foreach (var itemPrefab in itemPrefabs)
        {
            // Get the item data reference
            InventoryItemData itemData = itemPrefab.GetComponent<ItemObject>().referenceItem;

            // Check if the item is already in the inventory
            if (IsItemInInventory(itemData.id))
            {
               
                continue; // Skip spawning if the item is already in inventory
            }

            // Spawn the item if not in inventory
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
