using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject GameOverUI;
    private bool IsPaused = false;
    void Start()
    {
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
        }
        else
        { 
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
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
    }

    public void ResumeGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

}
