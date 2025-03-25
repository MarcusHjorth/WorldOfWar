using System;
using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
  public float interactDistance = 5f;
  public LayerMask interactables;
  public Camera playerCamera;

  void Start()
  {
    if (playerCamera == null)
    {
      Debug.LogError("Player Camera is NOT assigned! Check if the name is correct.");
    }
  }
  private void Update()
  {
    if (Input.GetKeyDown(KeyCode.E))
    {
      Debug.Log("E was pressed!");
      TryInteract();
    }
    // ✅ Allow the player to press "X" or "Escape" to close dialogue
    if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
    {
      CloseDialogue();
    }
  }

  void TryInteract()
  {
    Debug.Log("TryInteract() was called! Checking for interactables...");

    RaycastHit hit;
    
    // ✅ STEP 1: Get a safer starting position for the ray
    Vector3 rayOrigin = playerCamera.transform.position + playerCamera.transform.forward * 0.5f; // Moves origin slightly forward
    Vector3 rayDirection = playerCamera.transform.forward; // Keep it forward
    
    // ✅ STEP 2: Draw a Red Debug Ray in the Scene View for debugging
    Debug.DrawRay(rayOrigin, rayDirection * interactDistance, Color.red, 2f); 
    
    // ✅ STEP 3: Perform the Raycast
    if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactDistance, interactables))
    {
      Debug.Log("✅ Raycast hit: " + hit.collider.gameObject.name);

      IInteractable interactable = hit.collider.GetComponent<IInteractable>();
      if (interactable != null)
      {
        Debug.Log("✅ Interacting with: " + hit.collider.gameObject.name);
        interactable.Interact(gameObject);
      }
      else
      {
        Debug.LogError("⚠️ Raycast hit " + hit.collider.gameObject.name + ", but it has NO IInteractable component!");
      }
    }
    else
    {
      Debug.LogError("❌ Raycast didn't hit anything! The NPC might be missing a collider, or the ray origin is wrong.");
    }
  }
  public void CloseDialogue()
  {
    GameObject questPanel = GameObject.Find("QuestPanel");

    if (questPanel != null)
    {
      Debug.Log("✅ Close key was pressed! Hiding quest panel...");
      questPanel.SetActive(false);
    }
  }

}
