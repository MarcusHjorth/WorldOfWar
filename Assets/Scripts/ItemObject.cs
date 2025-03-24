using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void OnHandlePickItem()
    {
        // Calls a new method 
        InventorySystem.current.Add(referenceItem); // refenceItem from ItemObj 

        // Destorys the gameObj in the scene then  is's hit with reycast 
        Destroy(gameObject);
    }
}