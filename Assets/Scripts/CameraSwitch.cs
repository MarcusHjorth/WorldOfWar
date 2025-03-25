using System.Collections;
using UnityEngine;
using UnityEngine.UI;  // Add this for RawImage reference

public class CameraSwitch : MonoBehaviour
{
    public Camera introCamera;  // Reference to the main camera (intro)
    public Camera playerCamera; // Reference to the player's camera (gameplay)
    public float introDuration = 10f;  // Duration of the intro cutscene
    
    public RawImage crosshair;  // Reference to the crosshair (RawImage)
    
    private bool isIntroComplete = false;

    void Start()
    {
        // Initially, disable the player camera and enable the intro camera
        playerCamera.gameObject.SetActive(false);  // Disable player camera
        introCamera.gameObject.SetActive(true);  // Enable intro camera

        // Disable the crosshair during the intro
        if (crosshair != null)
        {
            crosshair.gameObject.SetActive(false);
        }

        // Start the cutscene coroutine to handle the intro
        StartCoroutine(PlayIntroCutscene());
    }

    private IEnumerator PlayIntroCutscene()
    {
        // Wait for the intro duration
        yield return new WaitForSeconds(introDuration);

        // After the intro, switch to the player camera
        SwitchToGameplay();
    }

    private void SwitchToGameplay()
    {
        // Switch to the gameplay camera
        introCamera.gameObject.SetActive(false);  // Disable intro camera
        playerCamera.gameObject.SetActive(true);  // Enable player camera

        // Optionally, enable the crosshair now that intro is complete
        if (crosshair != null)
        {
            crosshair.gameObject.SetActive(true);
        }

        isIntroComplete = true;
    }
}