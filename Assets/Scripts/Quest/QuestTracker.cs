using UnityEngine;
using UnityEngine.UI;

public class QuestTracker : MonoBehaviour
{
    public static QuestTracker instance;

    public int requiredSwords = 3;
    public int requiredAxes = 2;

    private int collectedSwords = 0;
    private int collectedAxes = 0;

    public GameObject progressPanel;
    public Text progressText; // Assign in Inspector
    public GameObject questPanel;

    private bool questAccepted = false;

    void Start()
    {
        if (progressPanel != null)
            progressPanel.SetActive(false);
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Update()
    {
        // 💡 Check if player presses ENTER while quest panel is open
        if (questPanel != null && questPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                AcceptQuest();
            }
        }
    }

    public void AcceptQuest()
    {
        if (questAccepted) return; // 💡 Prevent double-accepting

        questAccepted = true;
        collectedSwords = 0;
        collectedAxes = 0;
        UpdateUI();

        if (questPanel != null)
            questPanel.SetActive(false);

        if (progressPanel != null)
        {
            progressPanel.SetActive(true);
        }

        Debug.Log("🧭 Quest accepted!");
    }

    public void RegisterItem(string itemName)
    {
        if (!questAccepted) return;

        if (itemName.ToLower().Contains("sword"))
            collectedSwords++;

        if (itemName.ToLower().Contains("axe"))
            collectedAxes++;

        UpdateUI();

        if (collectedSwords >= requiredSwords && collectedAxes >= requiredAxes)
        {
            Debug.Log("🏆 QUEST COMPLETE!");
            // Optional: reward logic goes here
        }
    }

    void UpdateUI()
    {
        if (progressText != null)
        {
            progressText.text = $"Swords: {collectedSwords}/{requiredSwords} | Axes: {collectedAxes}/{requiredAxes}";
        }
    }
}