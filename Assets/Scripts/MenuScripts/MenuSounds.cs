using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSounds : MonoBehaviour
{
    [SerializeField] Slider MusicSlider;
    [SerializeField] AudioSource audioSource;
    public AudioMixer mixer;
    private const string MusicVolumeKeyM = "MusicVolume";
    public AudioClip Select;
    public AudioClip mouseEnter;






    // Start is called before the first frame update
    void Start()
    {
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKeyM, 1.0f);
        if (MusicSlider != null)
        {
            MusicSlider.value = savedMusicVolume;
        }
        SetMusic();
    }
    public void SetMusic()
    {
        if (mixer != null && MusicSlider != null)
        {
            float volume = MusicSlider.value;
            mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            MusicSlider.value = volume;
            PlayerPrefs.SetFloat(MusicVolumeKeyM, volume);
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Botao()
    {
        audioSource.PlayOneShot(Select);
    }
    
    
}
