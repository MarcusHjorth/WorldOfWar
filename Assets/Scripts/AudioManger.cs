using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("-------- Audio Sources --------")]
    [SerializeField] private AudioSource musicScource;

    [Header("-------- Audio Clip --------")]
    public AudioClip background;

    [Header("-------- Volume Control --------")]
    [Range(0f, 1f)] public float startVolume = 0.25f;  // Initial volume
    [Range(0f, 1f)] public float endVolume = 0.15f;    // Final volume
    public float fadeDuration = 5f; // Duration for the fade effect
    
    public AudioMixer theMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol"))
        {
            theMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol"));
        }

        if (PlayerPrefs.HasKey("MusicVol"))
        {
            theMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        }

        if (PlayerPrefs.HasKey("SFXVol"))
        {
            theMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("SFXVol"));
        }
        
        musicScource.clip = background;
        musicScource.volume = startVolume; // Set the volume to the start level
        musicScource.Play();

        // Start the fade-in process
        StartCoroutine(FadeInMusic(fadeDuration));
    }

    private IEnumerator FadeInMusic(float duration)
    {
        float startVol = musicScource.volume; // Capture the initial volume
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            // Gradually increase the volume over time
            musicScource.volume = Mathf.Lerp(startVol, endVolume, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the volume is set exactly to the final value
        musicScource.volume = endVolume;
    }

    // Optional: Method to update volume dynamically (for testing or changes during gameplay)
    public void SetMusicVolume(float volume)
    {
        musicScource.volume = volume;
    }
}