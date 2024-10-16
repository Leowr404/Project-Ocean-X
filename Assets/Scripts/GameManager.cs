using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    AudioManager audioManager;
    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    public GameObject BackGround;
    private bool IsPaused = false;
    [SerializeField]private Volume _Globalvolume;
    private DepthOfField depthOfField;
    private int Pontos;
    public GameObject PlacarPontos;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Time.timeScale = 1f;
        PauseMenuUI.SetActive(false);
        GameOverUI.SetActive(false);
        BackGround.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PauseGame()
    {
        if (IsPaused)
        {
            BackGround.SetActive(false);
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;  
            IsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
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
    public void PlayAgaion()
    {
        SceneManager.LoadScene("Gameplay");
        audioManager.PlaySFX(audioManager.Select);
    }

    public void ResumeGame()
    {
        BackGround.SetActive(false);
        PauseMenuUI.SetActive(false);
        audioManager.PlaySFX(audioManager.Select);
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

}
