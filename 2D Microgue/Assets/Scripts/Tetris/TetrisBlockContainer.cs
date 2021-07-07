using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisBlockContainer : MonoBehaviour
{
    public event Action BlockContainerMoved;
    public event Action BlockContainerRotated;
    public event Action<Vector3, float, float> BlockContainerStartedRotating;
    public static event Action BlockContainerPlaced;

    [SerializeField] float movementDuration  = 0.35f;
    [SerializeField] float animationDuration = 0.35f;

    [SerializeField] TetrisBlock blockPrefab;
    [SerializeField] List<TetrisBlock> blockList = new List<TetrisBlock>();
    [SerializeField] [Range(0.0f, 1.0f)] float blockSpawnChance = 0.5f;
    [SerializeField] List<Color> colorList = new List<Color>();
    Color blockContainerColor;

    bool isActive   = true;
    bool isMoving   = false;
    bool isRotating = false;

    const float RIGHT_ANGLE = 90f;

    void Start()
    {
        ChooseRandomColor();
        GenerateBlocks();
    }

    void ChooseRandomColor()
    {
        blockContainerColor = colorList[Random.Range(0, colorList.Count)];
    }

    void GenerateBlocks()
    {
        for (int x = -1; x < 2; x++)
        {
            for (int y = -1; y < 2; y++)
            {
                if (x == 0 && y == 0)
                {
                    GenerateBlock(x, y);
                }

                else
                {
                    float blockSpawnRoll = Random.Range(0.0f, 1.0f);

                    if (blockSpawnRoll < blockSpawnChance)
                    {
                        GenerateBlock(x, y);
                    }
                }
            }
        }
    }

    void GenerateBlock(int xPosition, int yPosition)
    {
        Vector3Int position = new Vector3Int((int) transform.position.x + xPosition, (int) transform.position.y + yPosition, 0);

        TetrisBlock neighborBlock = Instantiate(blockPrefab, position, Quaternion.identity);
        neighborBlock.blockPosition = position;
        neighborBlock.transform.SetParent(transform);
        
        SpriteRenderer blockSpriteRenderer = neighborBlock.GetComponent<SpriteRenderer>();
        blockSpriteRenderer.color = blockContainerColor;

        Debug.Log(neighborBlock.transform.parent);

        blockList.Add(neighborBlock);
    }

    bool BlockAlreadyExists(Vector3Int _blockPosition)
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (_blockPosition == blockList[i].blockPosition)
            {
                return true;
            }
        }

        return false;
    }

    # region Input Methods

    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow)       || Input.GetKeyDown(KeyCode.A)) { TweenMove(Vector3.left); }

            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) { TweenMove(Vector3.right); }

            else if (Input.GetKeyDown(KeyCode.UpArrow)    || Input.GetKeyDown(KeyCode.W)) { TweenMove(Vector3.up); }

            else if (Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S)) { TweenMove(Vector3.down); }

            if (Input.GetKeyDown(KeyCode.E)) { TweenRotate(Vector3.forward); }

            else if (Input.GetKeyDown(KeyCode.Q)) { TweenRotate(Vector3.back); }

            if (Input.GetKeyDown(KeyCode.Space)) { AttemptToPlaceBlocks(); }
        }
    }

    # endregion

    # region Movement Methods

    void TweenMove(Vector3 _directionToMove)
    {
        if (!MoveIsValid(_directionToMove.x, _directionToMove.y) || LeanTween.isTweening()) { return; } // TODO Fix instant movement issue
        
        isMoving = true;
        LeanTween.move(gameObject, transform.position += _directionToMove, movementDuration).setEaseInOutCubic().setOnComplete(OnMoveComplete);
        
    }

    bool MoveIsValid(float _offsetX, float _offsetY)
    {
        foreach (TetrisBlock block in blockList)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x + _offsetX);
            int roundedY = Mathf.RoundToInt(block.transform.position.y + _offsetY);

            if (OutsideGridBounds(roundedX, roundedY, 0, TetrisGrid.Instance.GridWidth, 0, TetrisGrid.Instance.GridHeight))
            {
                Debug.Log(this + " " + roundedX + ", " + roundedY + " is outside the grid.");
                return false;
            }
        }

        return true;
    }

    bool OutsideGridBounds(int _x, int _y, int _widthMin, int _widthMax, int _heightMin, int _heightMax)
    {
        return _x < _widthMin || _x >= _widthMax || _y < _heightMin || _y >= _heightMax;
    }

    void OnMoveComplete()
    {
        isMoving = false;
        BlockContainerMoved?.Invoke();
    }

    # endregion

    # region Rotation Methods

    void TweenRotate(Vector3 _directionToRotate)
    {
        if (!LeanTween.isTweening())
        {
            isRotating = true;
            LeanTween.rotateAround(gameObject, _directionToRotate, -RIGHT_ANGLE, movementDuration).setEaseInOutCubic().setOnComplete(OnRotateComplete);
            BlockContainerStartedRotating?.Invoke(_directionToRotate, -RIGHT_ANGLE, movementDuration);
        }
    }

    void OnRotateComplete()
    {
        isRotating = false;
        BlockContainerRotated?.Invoke();
    }

    # endregion

    # region Placement Methods

    void AttemptToPlaceBlocks()
    {
        if (ValidPlacement(out int overflowCount))
        {
            DiscardOverflowBlocks();
            AddBlocksToGrid();
            OnPlacementComplete();

            TetrisGameManager.Instance.IncreaseOverflowCount(overflowCount);
        }
    }

    bool ValidPlacement(out int _overflow)
    {
        _overflow = 0;

        foreach (TetrisBlock block in blockList)
        {
            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);

            if (GridSpaceIsOccupied(roundedX, roundedY))
            {
                Debug.Log(this + " " + roundedX + ", " + roundedY + " is already occupied.");
                return false;
            }

            if (OutsideGridBounds(roundedX, roundedY, 1, TetrisGrid.Instance.GridWidth - 1, 1, TetrisGrid.Instance.GridHeight - 1)) // Inner grid will allow placement but with overflow penalty
            {
                block.IsOverflow = true;
                _overflow++;
            }
        }

        return true;
    }

    bool GridSpaceIsOccupied(int _x, int _y)
    {
        return TetrisGrid.Instance.Grid[_x, _y] != null;
    }

    void AddBlocksToGrid()
    {
        foreach (TetrisBlock block in blockList)
        {
            if (block.IsOverflow) { return; }

            int roundedX = Mathf.RoundToInt(block.transform.position.x);
            int roundedY = Mathf.RoundToInt(block.transform.position.y);

            TetrisGrid.Instance.Grid[roundedX, roundedY] = block.transform;
        }
    }

    void DiscardOverflowBlocks()
    {
        foreach (TetrisBlock block in blockList) { if (block.IsOverflow) { block.TweenDiscard(); } }
    }

    void OnPlacementComplete()
    {
        isActive = false;
        BlockContainerPlaced?.Invoke();
    }

    # endregion
}
