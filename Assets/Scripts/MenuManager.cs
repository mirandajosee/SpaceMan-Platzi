using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager sharedInstance;
    public Canvas menuCanvas;
    public Canvas gameCanvas;
    public Canvas gameOverCanvas;

    private void Awake()
    {
        if (sharedInstance== null)
        {
            sharedInstance = this;
        }
    }
    public void ShowMainMenu()
    {
        menuCanvas.enabled = true;
    }
    public void HideMainMenu()
    {
        menuCanvas.enabled = false;
    }

    public void ShowGameUI()
    {
        gameCanvas.enabled = true;
    }
    public void HideGameUI()
    {
        gameCanvas.enabled = false;
    }

    public void ShowGameOver()
    {
        gameOverCanvas.enabled = true;
    }
    public void HideGameOver()
    {
        gameOverCanvas.enabled = false;
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
