using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public RectTransform[] buttons;  // Array com os botões
    public RectTransform[] Logos;  // Array com os botões
    //public RectTransform OptionMenu;
    //public Button buton;
    public float startX = -500f;     // Posição inicial no eixo X (fora da tela)
    //public float startY = 500f;     // Posição inicial no eixo X (fora da tela)
    [Header("Config Buttons")]
    public float animationDuration = 0.5f; // Duração da animação de cada botão
    public float delayBetweenButtons = 0.1f; // Atraso entre cada botão
    //
    [Header("Config Logos")]
    public float animationDurationlogo = 0.5f; // Duração da animação de cada botão
    public float delayBetweenLogos = 0.1f; // Atraso entre cada botão

    [Header("Config Effects")]
    public CanvasGroup flashCanvasGroup;           // Imagem do flash
    public GameObject particleEffect; // Partícula a ser iniciada
    public AudioSource audioSource;   // Fonte do som
    public AudioClip flashSound;      // Som que será tocado

    [Header("Config Options Menu")]
    public RectTransform optionsPanel;  // Painel de opções
    public RectTransform creditsPanel;

    [Header("Config Sounds")]
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider UiSlider;
    public AudioMixer mixer;
    private const string SFXVolumeKeyM = "SFXVolume";
    private const string MusicVolumeKeyM = "MusicVolume";
    public AudioClip select;
    public AudioClip close;
    public AudioClip mouseEnter;

    void Start()
    {
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKeyM, 1.0f);
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKeyM, 1.0f);
        if (UiSlider != null)
        {
            UiSlider.value = savedSFXVolume;
        }

        if (MusicSlider != null)
        {
            MusicSlider.value = savedMusicVolume;
        }
        AnimateButtons();
        AnimateLogos();
    }

    void AnimateButtons()
    {
        // Anima cada botão
        for (int i = 0; i < buttons.Length; i++)
        {
            RectTransform button = buttons[i];
            Vector3 originalPosition = button.anchoredPosition;

            // Define a posição inicial
            button.anchoredPosition = new Vector2(startX, originalPosition.y);

            // Move para a posição final com atraso baseado no índice
            button.DOAnchorPos(originalPosition, animationDuration)
                  .SetDelay(i * delayBetweenButtons)
                  .SetEase(Ease.OutBack); // Suaviza a entrada com rebote
        }
    }
    void AnimateLogos()
    {
        int completedAnimations = 0;

        for (int i = 0; i < Logos.Length; i++)
        {
            RectTransform logo = Logos[i];
            Vector3 originalPosition = logo.anchoredPosition;

            logo.anchoredPosition = new Vector2(startX, originalPosition.y);

            logo.DOAnchorPos(originalPosition, animationDurationlogo)
                .SetDelay(i * delayBetweenLogos)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    completedAnimations++;
                    if (completedAnimations == Logos.Length)
                    {
                        // Dispara os efeitos ao final da animação das logos
                        TriggerEffects();
                    }
                });
        }
    }
    public void TriggerEffects()
    {
        // Ativa o painel antes de iniciar o flash
        particleEffect.SetActive(true);
        flashCanvasGroup.gameObject.SetActive(true);

        // Flash: aumenta e reduz a opacidade usando CanvasGroup
        flashCanvasGroup.DOFade(1, 0.2f) // Aparece em 0.2 segundos
                        .OnComplete(() =>
                        {
                            flashCanvasGroup.DOFade(0, 0.5f) // Desaparece suavemente
                                            .OnComplete(() =>
                                            {
                                                // Desativa o painel após o fade-out
                                                flashCanvasGroup.gameObject.SetActive(false);
                                            });
                        });

        // Inicia a partícula
       // if (particleEffect != null)
         //   particleEffect.Play();

        // Toca o som
        if (audioSource != null && flashSound != null)
            audioSource.PlayOneShot(flashSound);
    }
    public void OpenOptions()
    {
        // Ativa o painel e inicia a animação de escala
        audioSource.PlayOneShot(select);
        optionsPanel.gameObject.SetActive(true);
        optionsPanel.localScale = Vector3.zero; // Garante que comece invisível
        optionsPanel.DOScale(Vector3.one, animationDuration)
                    .SetEase(Ease.OutBack); // Suaviza a entrada com um efeito rebote
    }
    public void CloseOptions()
    {
        // Inicia a animação de escala para 0 e desativa o painel ao final
        audioSource.PlayOneShot(close);
        optionsPanel.DOScale(Vector3.zero, animationDuration)
                    .SetEase(Ease.InBack) // Suaviza a saída com efeito de recolhimento
                    .OnComplete(() =>
                    {
                        optionsPanel.gameObject.SetActive(false);
                    });
    }
    public void OpenCredits()
    {
        // Ativa o painel e inicia a animação de escala
        audioSource.PlayOneShot(select);
        creditsPanel.gameObject.SetActive(true);
        creditsPanel.localScale = Vector3.zero; // Garante que comece invisível
        creditsPanel.DOScale(Vector3.one, animationDuration)
                    .SetEase(Ease.OutBack); // Suaviza a entrada com um efeito rebote
    }
    public void CloseCredits()
    {
        // Inicia a animação de escala para 0 e desativa o painel ao final
        audioSource.PlayOneShot(close);
        creditsPanel.DOScale(Vector3.zero, animationDuration)
                    .SetEase(Ease.InBack) // Suaviza a saída com efeito de recolhimento
                    .OnComplete(() =>
                    {
                        creditsPanel.gameObject.SetActive(false);
                    });
    }
    public void ParticleOn()
    {
        if(particleEffect.activeSelf == false) particleEffect.SetActive(true);
        else particleEffect.SetActive(false);
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
    public void SetUiSound()
    {
        if (mixer != null && UiSlider != null)
        {
            float volume = UiSlider.value;
            mixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
            UiSlider.value = volume;
            PlayerPrefs.SetFloat(SFXVolumeKeyM, volume);
            PlayerPrefs.Save();
        }
    }
    public void Botao()
    {
        audioSource.PlayOneShot(mouseEnter);
        
    }

}
