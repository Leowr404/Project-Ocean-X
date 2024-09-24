using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneMenu : MonoBehaviour
{
    public GameObject Options;

    // Start is called before the first frame update
    void Start()
    {
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
        SceneManager.LoadScene("Gameplay");
    }



}
