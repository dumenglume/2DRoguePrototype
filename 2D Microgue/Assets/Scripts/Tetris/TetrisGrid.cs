using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisGrid : MonoBehaviour
{
    public static event Action GridFull;
    public static event Action NoMovesLeft;

    [Header("Tetris Grid Settings")]
    [SerializeField] int gridWidth = 8;
    [SerializeField] int gridHeight = 6;
    public int GridWidth { get { return gridWidth; } set { gridWidth = value; } }
    public int GridHeight { get { return gridHeight; } set { gridHeight = value; } }

    Transform[,] grid;
    public Transform[,] Grid { get { return grid; } set { grid = value; } }

    static TetrisGrid instance;
    public static TetrisGrid Instance => instance;

    void Awake() 
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        grid = new Transform[gridWidth, gridHeight];
    }

    void OnDrawGizmos()
    {
/*        if (Application.isPlaying)
        {
            foreach(Transform cell in grid)
            {
                if (cell != null)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(cell.position, Vector3.one * 0.5f);
                }
            }
        }
*/    }
}
