using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;

public class LevelGrid 
{
    private Vector2Int foodGridPosition;
    private Vector2Int wallGridPosition;
    [SerializeField] private GameObject foodGameObject;
    private int levelNumber;
    
    private int width;
    private int height;
    private Snake snake;

    public List<Vector2Int> wallPositions = new List<Vector2Int>();

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(Snake snake)
    {
        this.snake = snake;

        //switch (levelNumber)
        //{
        //    case levelNumber1:
                
        //    case levelNumber2:
        //}
        
        
        
        // ------------ Wall List ---------------
        
        wallPositions.Add(new Vector2Int(1, 1));
        
        SpawnWalls();
        SpawnFood();
    }

    public void SpawnWalls()
    {
        // do
        // {
        //     wallGridPosition = new Vector2Int(Random.Range(1, width),Random.Range(1, height));
        // } while (snake.GetFullSnakeGridPositionList().IndexOf(wallGridPosition) != -1);

        foreach (Vector2Int wallGridPosition in wallPositions)
        {
            GameObject wallGameObject = new GameObject("Wall", typeof(SpriteRenderer));
            wallGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.wallSprite;
            wallGameObject.transform.position = new Vector3(wallGridPosition.x, wallGridPosition.y);
            wallGameObject.tag = "Wall";
        }
    }
    
    private void SpawnFood()
    {
        do
        {
            foodGridPosition = new Vector2Int(Random.Range(1, width), Random.Range(1, height));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1 ||
                 wallPositions.IndexOf(foodGridPosition) != -1);
        

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y);
        foodGameObject.tag = "Food";
    }
    
    public bool SnakeEatFood(Vector2Int snakeGridPosition)
    {
        if(snakeGridPosition == foodGridPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            GameHandler.AddScore();
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool SnakeTossWall(Vector2Int snakeGridPosition)
    {
        if(wallPositions.IndexOf(snakeGridPosition) != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Vector2Int ValidateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 1)
        {
            gridPosition.x = width - 1;
        }
        
        if (gridPosition.x > width - 1)
        {
            gridPosition.x = 1;
        }
        
        if (gridPosition.y < 1)
        {
            gridPosition.y = height - 1;
        }
        
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 1;
        }

        return gridPosition;
    }
}
