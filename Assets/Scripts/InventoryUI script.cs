using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // reference inventory panel
    public Transform itemContainer; // reference container 
    public GameObject itemSlotPrefab; // reference to prefab ItemSlot UI 
    private bool isOpen = false; // if the UI is open or not 

    private void Start()
    {
        inventoryPanel.SetActive(false);   // Can't see UI when stating the game 
        InventorySystem.current.onInventoryChanged += UpdateUI; // listens for changes to the inventory via the onInventoryChanged event 
    }

    
    public void ToggleInventory()
    {
        // Toggle the inventory state (open or close) 
        isOpen = !isOpen;   
        inventoryPanel.SetActive(isOpen); 

        // shows mosue cursor when open. 
        Cursor.visible = isOpen; 
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    
    public bool IsInventoryOpen()
    {
        return isOpen;  // Returns whether the inventory is currently open or not
    }

    public void UpdateUI(List<InventoryItem> inventory)
    {

        foreach (Transform child in itemContainer)  // loops over existing child objects in ItemConatiner 
        {
            Destroy(child.gameObject);  // removes each item slot from the UI aka clearing out any old items befor updating 
        }

       
        foreach (InventoryItem item in inventory)   // Loops over inventoryItems 
        {

           
            GameObject newItemSlot = Instantiate(itemSlotPrefab, itemContainer);    // Create new itemslot for item 

            // makes the itemslot to the image 
            Image itemImage = newItemSlot.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.data.icone; // sprite = 2D image or background 
            }

            Transform stackSizeTransform = newItemSlot.transform.Find("StackSizeText");
            if (stackSizeTransform != null)
            {
                TextMeshProUGUI stackSizeText = stackSizeTransform.GetComponent<TextMeshProUGUI>();
                if (stackSizeText != null)
                {
                    stackSizeText.text = item.stackSize > 1 ? item.stackSize.ToString() : ""; 
                }
                
            }
            
        }
    }
}
    

