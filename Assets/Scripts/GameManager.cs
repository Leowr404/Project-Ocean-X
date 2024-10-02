using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    AudioManager audioManager;
    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    private bool IsPaused = false;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        Time.timeScale = 1f;
        PauseMenuUI.SetActive(false);
        //GameOverUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PauseGame()
    {
        if (IsPaused)
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1f;  
            IsPaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseGame();
        }
    }

    public void CarregarMenu()
    {
        SceneManager.LoadScene("Menu");
        audioManager.PlaySFX(audioManager.Select);
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        audioManager.PlaySFX(audioManager.Select);
        Time.timeScale = 1f;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
