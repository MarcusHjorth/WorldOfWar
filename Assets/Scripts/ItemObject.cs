using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void OnHandlePickItem()
    {
        InventorySystem.current.Add(referenceItem);
        Destroy(gameObject);
    }
}