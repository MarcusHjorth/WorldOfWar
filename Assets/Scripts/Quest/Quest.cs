using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public string questTitle = "Mission Briefing"; // Quest title
    [TextArea] public string questDescription = "Your task is to gather resources and return to base."; // Quest details

    public GameObject questPanel; // The UI panel
    public Text titleText; // UI text for quest title
    public Text descriptionText; // UI text for quest description

    public void ShowQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(true);
            titleText.text = questTitle; // Set title
            descriptionText.text = questDescription; // Set description
        }
    }

    public void CloseQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
        }
    }
}