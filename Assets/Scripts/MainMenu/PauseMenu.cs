using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    private bool isPaused = false;

    public WowCamera camera;
    public PlayerMovement playerMovement;

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

        if (camera != null && playerMovement != null)
        {
            camera.enabled = false;
            playerMovement.enabled = false;
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

        if (camera != null && playerMovement != null)
        {
            camera.enabled = true;
            playerMovement.enabled = true;
        }

        isPaused = false;
        Debug.Log("Game resumed, time unfreeze");
    }
}