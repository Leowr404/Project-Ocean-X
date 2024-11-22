using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using DG.Tweening;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    AudioManager audioManager;
    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    public GameObject BackGround;
    public GameObject WinUI;
    private bool IsPaused = false;
    [SerializeField]private Volume _Globalvolume;
    private DepthOfField depthOfField;
    private int Pontos;
    //public GameObject PlacarPontos;
    //
    [SerializeField] RectTransform PausePainel;
    [SerializeField] float topPosY, middlePosY;
    [SerializeField] float tweedurat;

    public void Awake()
    {
        Instance = this;
    }
    async void Start()
    {
        await PauseOut();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Time.timeScale = 1f;
        PauseMenuUI.SetActive(false);
        GameOverUI.SetActive(false);
        BackGround.SetActive(false);
        WinUI.SetActive(false);
        Debug.Log("Jogo Inicou Corretamente");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PauseGame()
    {
        if (IsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseInto();
            BackGround.SetActive(true);
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
        }
    }

    public void CarregarMenu()
    {
        SceneManager.LoadScene("Menu");
        audioManager.PlaySFX(audioManager.Select);
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("Gameplay2");
        audioManager.PlaySFX(audioManager.Select);
    }
    public void PlayAgaion()
    {
        SceneManager.LoadScene("Gameplay");
        audioManager.PlaySFX(audioManager.Select);
    }
    public void PlayAgaion2()
    {
        SceneManager.LoadScene("Gameplay2");
        audioManager.PlaySFX(audioManager.Select);
    }

    public async void ResumeGame()
    {
        await PauseOut();
        BackGround.SetActive(false);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void GameOver()
    {
        BackGround.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameOverUI.SetActive(true);
        Time.timeScale = 0f;
        

    }
    public void WinLevel()
    {
        BackGround.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        WinUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ProfundidadeON()
    {
        depthOfField.active = true;
    }
    public void ProfundidadeOFF()
    {
        depthOfField.active = false;
    }

    public void Mostrarpontos()
    {
        Pontos.ToString();
    }

    public void PauseInto()
    {
        PausePainel.DOAnchorPosY(middlePosY, tweedurat).SetUpdate(true);
    }

    async Task PauseOut()
    {
      await PausePainel.DOAnchorPosY(topPosY, tweedurat).SetUpdate(true).AsyncWaitForCompletion();
    }

}
