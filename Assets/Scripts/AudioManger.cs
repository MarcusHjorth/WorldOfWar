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
    [Range(0f, 1f)] public float startVolume = 0.25f; 
    [Range(0f, 1f)] public float endVolume = 0.15f;   
    public float fadeDuration = 5f; 

    private void Start()
    {
        musicScource.clip = background;
        musicScource.volume = startVolume; 
        musicScource.Play();

       
        StartCoroutine(FadeOutMusic(fadeDuration));
    }

    /* 
    IEnumerator is a return type in unity for coroutines. 
    Coroutines allows to "pause execution" after a delay with freezing the game. 

    Yield return = pause the code until next frame. 
    changes the volume frame by frame  that give a "fade" effect. 

    */

    private IEnumerator FadeOutMusic(float duration)
    {
        float startVol = musicScource.volume; 
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            
            musicScource.volume = Mathf.Lerp(startVol, endVolume, timeElapsed / duration);  //mathf.lerp is smooth transitions between two values over time.
            timeElapsed += Time.deltaTime;
            yield return null;
        }

       
        musicScource.volume = endVolume;
    }

   
    public void SetMusicVolume(float volume)
    {
        musicScource.volume = volume;
    }
}