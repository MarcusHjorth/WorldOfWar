using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel; // The inventory UI panel
    public Transform itemContainer; // Where UI items are stored (parent of item slots)
    public GameObject itemSlotPrefab; // The prefab for item slots
    private bool isOpen = false; // Inventory visibility state

    private void Start()
    {
        inventoryPanel.SetActive(false); // Hide inventory at start
        InventorySystem.current.onInventoryChanged += UpdateUI; // Subscribe to inventory change
    }

    // Toggle the inventory open/close
    public void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen); // Show or hide the inventory panel based on the isOpen flag

        // Control cursor visibility and lock state
        Cursor.visible = isOpen; // Show cursor when inventory is open
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    // Public method to check if the inventory is open
    public bool IsInventoryOpen()
    {
        return isOpen;
    }

    // Update UI based on inventory content
    public void UpdateUI(List<InventoryItem> inventory)
    {
        Debug.Log("UpdateUI called! Refreshing inventory UI...");

        // Clear existing UI items
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        // Add updated inventory items
        foreach (InventoryItem item in inventory)
        {
            Debug.Log($"Item in inventory: {item.data.displayName}, Stack Size: {item.stackSize}");

            // Create new item slot (this is your ItemSlotPrefab)
            GameObject newItemSlot = Instantiate(itemSlotPrefab, itemContainer);

            // Set item icon (Image is used as a placeholder for the icon)
            Image itemImage = newItemSlot.GetComponent<Image>();
            if (itemImage != null)
            {
                itemImage.sprite = item.data.icone;
            }

            // Find the StackSizeText GameObject (which is a child of ItemSlotPrefab)
            // Set StackSizeText
            Transform stackSizeTransform = newItemSlot.transform.Find("StackSizeText");
            if (stackSizeTransform != null)
            {
                TextMeshProUGUI stackSizeText = stackSizeTransform.GetComponent<TextMeshProUGUI>();
                if (stackSizeText != null)
                {
                    stackSizeText.text = item.stackSize > 1 ? item.stackSize.ToString() : ""; // If stack size is 1, leave it empty
                }
                else
                {
                    Debug.LogError("TextMeshProUGUI component is missing on the StackSizeText object!");
                }
            }
            else
            {
                Debug.LogError("StackSizeText object missing in the item slot prefab!");
            }
        }
    }
}
    

