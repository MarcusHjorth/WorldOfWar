using UnityEngine;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPaused = false;

    public DemoPlayerMovement demoPlayerMovement;
    public DemoPlayerController demoPlayerController;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (demoPlayerMovement != null && demoPlayerController != null)
        {
            demoPlayerMovement.enabled = false;
            demoPlayerController.enabled = false;
        }

        isPaused = true;
        Debug.Log("Game paused, time freeze");
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (demoPlayerMovement != null && demoPlayerController != null)
        {
            demoPlayerMovement.enabled = true;
            demoPlayerController.enabled = true;
        }

        isPaused = false;
        Debug.Log("Game resumed, time unfreeze");
    }
}