using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour 
{
    private static GameHandler instance;
    
    [SerializeField] private Snake snake;
    [SerializeField] private GameObject objectiveWindow;
    [SerializeField] private GameObject completeWindow;
    [SerializeField] private int finishScore;
    private static int score;

    private LevelGrid levelGrid;
    
    private void Awake()
    {
        instance = this;
        InitializeStatic();
        Time.timeScale = 0;
    }

    private void Start() {
        Debug.Log("GameHandler.Start");

        levelGrid = new LevelGrid(20, 20);

        snake.Setup(levelGrid);
        
        levelGrid.Setup(snake);
        
        objectiveWindow.SetActive(true);
    }

    private static void InitializeStatic()
    {
        score = 0;
    }
    
    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 1;
    }

    public void CompleteLevel()
    {
        if (score == finishScore)
        {
            Time.timeScale = 0;
            completeWindow.SetActive(true);
        }
    }

    public static void SnakeDied()
    {
        GameOverWindow.ShowStatic();
    }
    
    public void StartGame()
    {
        Time.timeScale = 1;
        objectiveWindow.SetActive(false);
    }

    public void ChangeScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    
    
    
}
