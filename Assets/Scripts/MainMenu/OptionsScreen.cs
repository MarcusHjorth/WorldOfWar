using UnityEngine;
using UnityEngine.UI;
using Quaternion = System.Numerics.Quaternion;

public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyGraphics()
    {
        Screen.fullScreen = fullscreenTog.isOn;
        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }
}
