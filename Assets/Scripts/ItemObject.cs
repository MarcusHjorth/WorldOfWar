using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void OnHandlePickItem()
    {

        // If it's a weapon, equip it and add it to inventory to track it
        if (referenceItem.isWeapon)
        {
            PlayerEquipment.instance.EquipWeapon(referenceItem);
        }
        
        // Add non-weapons to the inventory
        InventorySystem.current.Add(referenceItem);
        
        // 👉 Track the item for quest progress
        QuestTracker.instance.RegisterItem(referenceItem.displayName);
        

        // Destroy the game object after picking up the item
        Destroy(gameObject);
    }
}