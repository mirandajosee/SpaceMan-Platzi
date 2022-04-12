using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    pause,
    gameOver
}

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance;
    private PlayerController controller;
    public int collectedObject = 0;
    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
    }

    void Start()
    {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
        MenuManager.sharedInstance.ShowMainMenu();
        BackToMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") &&
           currentGameState != GameState.inGame)
        {
        }
        if (Input.GetKeyDown(KeyCode.Q) &&  currentGameState == GameState.inGame)
        {
            MenuManager.sharedInstance.PauseGame();
            currentGameState = GameState.pause;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && currentGameState == GameState.pause)
        {
            MenuManager.sharedInstance.ResumeGame();
            currentGameState = GameState.inGame;
        }
    }


    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }
    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }
    private void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {
            MenuManager.sharedInstance.HideGameOver();
            MenuManager.sharedInstance.HideGameUI();
            MenuManager.sharedInstance.ShowMainMenu();
        }
        else if (newGameState == GameState.inGame)
        {
            LevelManager.sharedInstance.RemoveAllLevelBlocks();
            LevelManager.sharedInstance.GenerateInitialBlocks();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameOver();
            controller.StartGame();
            MenuManager.sharedInstance.ShowGameUI();
        }
        else if (newGameState == GameState.pause)
        {

        }
        else if (newGameState == GameState.gameOver)
        {
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameUI();
            MenuManager.sharedInstance.ShowGameOver();
        }
        this.currentGameState = newGameState;
    }

    public void CollectObject(Collectable collectable)
    {
        collectedObject += collectable.value;
    }
}
