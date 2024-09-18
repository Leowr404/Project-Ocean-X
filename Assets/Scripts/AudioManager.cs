using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Slider MusicSlider;
    public static AudioManager instancia;
    private const string SFXVolumeKey = "SFXVolume";
    private const string MusicVolumeKey = "MusicVolume";


    [Header("Configuracoes de Sons")]
    //public AudioClip bgmSound;
    public AudioClip Audio_PowerUp;
    public AudioClip Audio_Coletavel;
    public AudioClip Audio_Morte;
    public AudioClip Audio_Pause;
    public AudioClip Audio_Cheat;
    public AudioClip Shoot_sound;
    public AudioClip Buy_Loja;
    public AudioClip Equip;
    public AudioClip Death;
    public AudioClip backgroundMusicMenu;
    public AudioClip backgroundMusicGameplay;
    private AudioSource backgroundMusicSource;

    void Start()
    {
        SetupBackgroundMusic();
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1.0f);
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);

        if (SFXSlider != null)
        {
            SFXSlider.value = savedSFXVolume;
        }

        if (MusicSlider != null)
        {
            MusicSlider.value = savedMusicVolume;
        }

        // Configurar os volumes iniciais
        SetMusicSFX();
        SetMusic();
    }

    public void SetMusicSFX()
    {
        if (mixer != null && SFXSlider != null)
        {
            float volume = SFXSlider.value;
            mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            SFXSlider.value = volume;
            PlayerPrefs.SetFloat(SFXVolumeKey, volume);
            PlayerPrefs.Save();
        }

    }

    public void SetMusic()
    {
        if (mixer != null && MusicSlider != null)
        {
            float volume = MusicSlider.value;
            mixer.SetFloat("Music", Mathf.Log10(volume) * 20);
            MusicSlider.value = volume;
            backgroundMusicSource.volume = volume;
            PlayerPrefs.SetFloat(MusicVolumeKey, volume);
            PlayerPrefs.Save();
        }

    }
    private void SetupBackgroundMusic()
    {
        // Se tiver um som de fundo configurado
        if (backgroundMusicSource == null)
        {
            backgroundMusicSource = gameObject.AddComponent<AudioSource>();
            backgroundMusicSource.loop = true;
            backgroundMusicSource.playOnAwake = true;
        }

        // Determina a cena atual
        Scene currentScene = SceneManager.GetActiveScene();

        // Configura o AudioClip com base na cena
        if (currentScene.name == "Menu" && backgroundMusicMenu != null)
        {
            backgroundMusicSource.clip = backgroundMusicMenu;
        }
        else if (currentScene.name == "Gameplay" && backgroundMusicGameplay != null)
        {
            backgroundMusicSource.clip = backgroundMusicGameplay;
        }

        // Configura o volume e o mixer
        backgroundMusicSource.volume = PlayerPrefs.GetFloat(MusicVolumeKey, 1.0f);
        backgroundMusicSource.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];

        // Inicia a reprodução do som de fundo
        backgroundMusicSource.Play();
    }

}
