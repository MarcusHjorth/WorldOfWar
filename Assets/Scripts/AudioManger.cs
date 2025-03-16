using UnityEngine;

public class AudioManger : MonoBehaviour
{
    [Header("-------- Audio Sources --------")]
    [SerializeField] private AudioSource musicScource ;

    [Header("-------- Audio Clip --------")]
    public AudioClip background;

    private void Start()
    {
        musicScource.clip = background;
        musicScource.Play();
    }

}
