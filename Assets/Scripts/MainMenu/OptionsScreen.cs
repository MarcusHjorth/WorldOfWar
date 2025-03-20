using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using Quaternion = System.Numerics.Quaternion;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;
    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;
    public TMP_Text resolutionLabel;

    public AudioMixer theMixer;
    public TMP_Text masterLabel, musicLabel, sfxLabel;
    public Slider masterSlider, musicSlider, sfxSlider;

    public GameObject applyGraphicsPopupPanel;
    private bool graphicsApplied = true;

    private bool originalFullscreen;
    private int originalVSyncCount;
    private int originalResolutionIndex;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalFullscreen = Screen.fullScreen;
        originalVSyncCount = QualitySettings.vSyncCount;
        
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;
                selectedResolution = i;
                UpdateResLabel();
                originalResolutionIndex = i;
                break;
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;
            
            UpdateResLabel();

            originalResolutionIndex = selectedResolution;
        }

        float vol = 0f;
        theMixer.GetFloat("MasterVol", out vol);
        masterSlider.value = vol;
        
        theMixer.GetFloat("MusicVol", out vol);
        musicSlider.value = vol;

        theMixer.GetFloat("SFXVol", out vol);
        sfxSlider.value = vol;
        
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();
        
        // Ensure the warning pop-up is hidden at start.
        if (applyGraphicsPopupPanel != null)
            applyGraphicsPopupPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ResLeft()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = 0;
        }

        UpdateResLabel();
        graphicsApplied = false;
    }

    public void ResRight()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = resolutions.Count - 1;
        }

        UpdateResLabel();
        graphicsApplied = false;
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " +
                               resolutions[selectedResolution].vertical.ToString();
    }

    public void ToggleFullscreen()
    {
        graphicsApplied = false;
    }

    public void ToggleVSync()
    {
        graphicsApplied = false;
    }

    public void ApplyGraphics()
    {
        //Screen.fullScreen = fullscreenTog.isOn;
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical,
            fullscreenTog.isOn);

        graphicsApplied = true;
    }

    public void setMasterVol()
    {
        masterLabel.text = Mathf.RoundToInt(masterSlider.value + 80).ToString();

        theMixer.SetFloat("MasterVol", masterSlider.value);
        
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
    }

    public void setMusicVol()
    {
        musicLabel.text = Mathf.RoundToInt(musicSlider.value + 80).ToString();

        theMixer.SetFloat("MusicVol", musicSlider.value);
        
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
    }

    public void setSFXVol()
    {
        sfxLabel.text = Mathf.RoundToInt(sfxSlider.value + 80).ToString();

        theMixer.SetFloat("SFXVol", sfxSlider.value);
        
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
    }

    public void TryCloseOptions()
    {
        if (!graphicsApplied)
        {
            //If settings have not been applied, show the pop-up panel
            if (applyGraphicsPopupPanel != null)
                applyGraphicsPopupPanel.SetActive(true);
            else
                Debug.Log("Pop-up panel is not assigned");
        }
        else
        {
            //If no changes have been made to any settings, simply close this gameObject (options screen)
            gameObject.SetActive(false);
            Debug.Log("No changes, closing options screen...");
        }
    }

    public void ApplyAndClose()
    {
        ApplyGraphics();
        if (applyGraphicsPopupPanel != null)
            applyGraphicsPopupPanel.SetActive(false);
    }

    // Called by the Cancel button on the pop-up.
    // This will revert any pending changes back to the original settings.
    public void CancelAndReturnToOptions()
    {
        // Revert the actual graphics settings.
        Screen.SetResolution(resolutions[originalResolutionIndex].horizontal, resolutions[originalResolutionIndex].vertical,
            originalFullscreen);
        QualitySettings.vSyncCount = originalVSyncCount;
        
        // Revert the UI toggles and label.
        fullscreenTog.isOn = originalFullscreen;
        vsyncTog.isOn = (originalVSyncCount != 0);
        selectedResolution = originalResolutionIndex;
        UpdateResLabel();
        
        // Mark changes as applied.
        graphicsApplied = true;
        
        if (applyGraphicsPopupPanel != null)
            applyGraphicsPopupPanel.SetActive(false);
    }
}

[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}