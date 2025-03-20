using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPaused = false;

    // Update() is called once per frame.
    // This is where we check if the player has pressed a key.
    void Update()
    {
        // Check if the "L" key was pressed during this frame.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Check if the pause menu is currently active (visible).
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Debug.Log("Game paused, time freeze");
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Debug.Log("Game resumed, time unfreeze");
    }
}