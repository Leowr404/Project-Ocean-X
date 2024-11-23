using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneMenu : MonoBehaviour
{
    public GameObject Options;
    public CanvasGroup fadeCanvasGroup; // CanvasGroup usado para o fade
    public float fadeDuration = 2f;     // Duração do fade
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip startGame;

    // Start is called before the first frame update
    void Start()
    {
        fadeCanvasGroup.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Options.SetActive(false);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AbrirMenu()
    {
        Options.SetActive(true);
    }

    public void FecharMenu()
    {
        Options.SetActive(false);
    }

    public void PlayGame()
    {
        fadeCanvasGroup.gameObject.SetActive(true);
        audioSource.PlayOneShot(startGame);
        fadeCanvasGroup.DOFade(1, fadeDuration)
            .OnComplete(() =>
            {
                SceneManager.LoadScene("Gameplay");
            });
    }

   public void ExitGame() 
    {
        Application.Quit();
        Debug.Log("jogo fechado");

    }

    public void OnMouseOver()
    {
        Debug.Log("MOUSE SOBRE BOTAO");
        
    }




}
