using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void OnHandlePickItem()
    {
        if (referenceItem == null)
        {
            Debug.LogError("Reference item is null! Cannot handle the item.");
            return;
        }

        // If it's a weapon, equip it and add it to inventory to track it
        if (referenceItem.isWeapon)
        {
            PlayerEquipment.instance.EquipWeapon(referenceItem);
        }
        
        // Add non-weapons to the inventory
        InventorySystem.current.Add(referenceItem);
        

        // Destroy the game object after picking up the item
        Destroy(gameObject);
    }
}