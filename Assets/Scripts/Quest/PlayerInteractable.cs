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
    if (Input.GetKeyDown(KeyCode.F))
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
    RaycastHit hit;
    
    Vector3 rayOrigin = playerCamera.transform.position + playerCamera.transform.forward * 0.5f; // Moves origin slightly forward
    Vector3 rayDirection = playerCamera.transform.forward; // Keep it forward
    
    // Draw a Red Debug Ray in the Scene View for debugging
    Debug.DrawRay(rayOrigin, rayDirection * interactDistance, Color.red, 2f); 
    
    //Perform the Raycast
    if (Physics.Raycast(rayOrigin, rayDirection, out hit, interactDistance, interactables))
    {
      IInteractable interactable = hit.collider.GetComponent<IInteractable>();
      if (interactable != null)
      {
        interactable.Interact(gameObject);
      }
    }
  }
  public void CloseDialogue()
  {
    GameObject questPanel = GameObject.Find("QuestPanel");

    if (questPanel != null)
    {
      questPanel.SetActive(false);
    }
  }

}
