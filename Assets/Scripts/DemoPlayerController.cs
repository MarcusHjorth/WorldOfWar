using System;
using UnityEngine;

public class DemoPlayerController : MonoBehaviour
{
    public float attackDamage = 10f;
    public float reach = 4f;

    private Health _health;

    // Reference to InventoryUI
    public InventoryUI inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        // Setup health system
        _health = GetComponent<Health>();
        _health.OnDie += OnDie;

        // Set the cursor locked and invisible at the start (normal game mode)
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            Hit();
        }

       
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            TryInteract();
        }

       
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryUI.ToggleInventory();
        }
    }

    // Method to handle the player's attack
    void Hit()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, reach))
        {
            if (hit.transform.TryGetComponent(out Health health))
            {
                health.Damage(attackDamage);
            }
        }
    }

    

   
    void TryInteract()
    {

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, reach))
        {
            if (hit.transform.TryGetComponent(out ItemObject item))
            {
                item.OnHandlePickItem(); 

                 /*
                    We use raycasting to determine where the mouse is pointing in the game world

                    When we hover over an Item (gameObj) and press E, we check with ray-cast if we hit an item. 
                    if yes = calls the method OnHandlePickItem()

                */

            }
        }
    }

    // Method to handle player death (when health reaches zero)
    void OnDie()
    {
        Debug.Log("Player died");
    }

    // Method to visualize the player's attack reach in the scene view (for debugging)
    private void OnDrawGizmos()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * reach);
    }
}
