using UnityEngine;

public class Volume : MonoBehaviour
{
    private AudioSource audioSrc;
    private float musicVolume = 0.3f;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSrc.volume = musicVolume;
    }

    public void SetVolume(float vol)
    {
        musicVolume = vol;
    }
}
