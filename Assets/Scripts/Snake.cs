using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class Snake : MonoBehaviour
{
    //---------------------------------------------   Directions   ---------------------------------------------//
    
    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }
    
    //---------------------------------------------   States  ---------------------------------------------//
    
    public enum State
    {
        Alive,
        Dead
    }
    
    //---------------------------------------------   Variables   ---------------------------------------------//
    
    public State state;
    private Vector2Int gridPosition;
    private Direction gridMoveDirection;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private Vector3 snakeBodyPosition;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    

    //---------------------------------------------   Setup   ---------------------------------------------//
    
    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    //---------------------------------------------   Awake   ---------------------------------------------//

    private void Awake()
    {   
        gridPosition = new Vector2Int(10,10);
        gridMoveTimerMax = 0.3f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
    }

    //---------------------------------------------   Update   ---------------------------------------------//
    
    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }
    }
    
    //---------------------------------------------   Keyboard Movements   ---------------------------------------------//
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)||Input.GetKeyDown(KeyCode.W))
        {
            if(gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
           
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)||Input.GetKeyDown(KeyCode.S))
        {
            if(gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)||Input.GetKeyDown(KeyCode.D))
        {
            if(gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)||Input.GetKeyDown(KeyCode.A))
        {
            if(gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        }
    }
    
    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;

        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right : gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left :  gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up :    gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down :  gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);

            bool snakeAteFood = levelGrid.SnakeEatFood(gridPosition);
            bool snakeTossWall = levelGrid.SnakeTossWall(gridPosition);
            
            if (snakeTossWall)
            {
                state = State.Dead;
                GameHandler.SnakeDied();
            }
            
            if(snakeAteFood)
            {
                snakeBodySize++;
                //gridMoveTimerMax -= 0.003f; //Hızlandırma
                CreateSnakeBodyPart();
            }

            if(snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }
            
            UpdateSnakeBodyParts();
            
            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    state = State.Dead;
                    GameHandler.SnakeDied();
                }
            }
            
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) + 90);
        }
    }
    
    
    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }
    
    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i]. SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }
    
    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() {gridPosition};
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition()); 
        }
        return gridPositionList;
    }
    
    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up: //Yukarı Dönünce
                    switch (snakeMovePosition.GetPreviousDirektion())
                    {
                        default:
                            angle = 180; break;
                        case Direction.Left:
                            angle = 180 + 45; break; //Önceki Sol ise
                        case Direction.Right:
                            angle = 180 - 45; break; //Önceki Sağ ise
                    }
                    break;
                case Direction.Down: //Aşağı Dönünce
                    switch (snakeMovePosition.GetPreviousDirektion())
                    {
                        default:
                            angle = 0; break;
                        case Direction.Left:
                            angle = 0 - 45; break; //Önceki Sol ise
                        case Direction.Right:
                            angle = 0 + 45; break; //Önceki Sağ ise
                    }
                    break;
                case Direction.Left: //Sola Dönünce
                    switch (snakeMovePosition.GetPreviousDirektion())
                    {
                        default:
                            angle = -90; break;
                        case Direction.Down:
                            angle = -90 + 45; break; //Önceki Aşağı ise
                        case Direction.Up:
                            angle = -90 - 45; break; //Önceki Yukarı ise
                    }
                    break;
                case Direction.Right: //Sağa Dönünce
                    switch (snakeMovePosition.GetPreviousDirektion())
                    {
                        default:
                            angle = 90; break;
                        case Direction.Down:
                            angle = 90 - 45; break; //Önceki Aşağı ise
                        case Direction.Up:
                            angle = 90 + 45; break; //Önceki Yukarı ise
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }
        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }
    
    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition,Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirektion()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
            
        }
    }
    
    //---------------------------------------------   Button Movements   ---------------------------------------------//
    
    public void TurnUp()
    {
        if(gridMoveDirection != Direction.Down)
        {
            gridMoveDirection = Direction.Up;
        }
    }
    public void TurnDown()
    {
        if(gridMoveDirection != Direction.Up)
        {
            gridMoveDirection = Direction.Down;
        }
    }
    public void TurnRight()
    {
        if(gridMoveDirection != Direction.Left)
        {
            gridMoveDirection = Direction.Right;
        }
    }
    public void TurnLeft()
    {
        if(gridMoveDirection != Direction.Right)
        {
            gridMoveDirection = Direction.Left;
        }
    }
   
}
