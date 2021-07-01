using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomWalkGenerator : MonoBehaviour
{
    [SerializeField] TileBase tile;
    [SerializeField] Tilemap tilemap;

    List<Vector2Int> directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    int gridCount = 20;
    int gridSpacing = 1;
    int gridSteps = 1000;
    List<TileBase> grid;

    void Start()
    {
        CreateLevel();
    }

    void CreateLevel()
    {
        Vector2Int currentPosition = Vector2Int.zero;

        Vector2Int currentDirection = Vector2Int.right;
        Vector2Int lastDirection = currentDirection * -1;

        for (int i = 0; i < gridCount; i++)
        {
            int randomIndex = Random.Range(0, directions.Count - 1);
            Vector2Int thisDirection = directions[randomIndex];

            while (Mathf.Abs(currentPosition.x + thisDirection.x) < gridCount || Mathf.Abs(currentPosition.y + thisDirection.y) < gridCount)
            {
                randomIndex = Random.Range(0, directions.Count - 1);
                thisDirection = directions[randomIndex];

                currentPosition += thisDirection * gridSpacing;
                lastDirection = thisDirection;

                Vector3Int tilePosition = new Vector3Int(currentPosition.x, currentPosition.y, 0);
                tilemap.SetTile(tilePosition, tile);
                // grid.Add(tile);
            }

            gridCount ++;
        }
    }

    void ShuffleList(List<Vector2Int> _list) // TODO Switch to generic list?
    {
        for (int i = 0; i < _list.Count; i++) 
        {
            Vector2Int temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
    }
}
