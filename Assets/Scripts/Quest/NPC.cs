using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteractable
{
    public string npcName= "Quest Giver";
    public UnityEvent interactionEvent; // Unity event for interactions
    [FormerlySerializedAs("dialoguePanel")] public GameObject questPanel; // Assign the UI panel in Unity
    [FormerlySerializedAs("dialogueText")] public Text questText;

    public void Interact(GameObject sender)
    {
        Debug.Log(npcName + " says: Hello, traveler!");

        if (questPanel != null)
        {
            questPanel.SetActive(true);

            if (questText != null)
            {
                questText.text = "Greetings traveler! I need you to pick up some axes and swords for me! Can you do that?"; // ✅ Set the text dynamically
                interactionEvent.Invoke();
            }
        }
        
    }
}
