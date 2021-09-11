using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HopRogue.Grid;

namespace HopRogue
{
public class DebugDungeon : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] bool _debugEnabled = true;
    [SerializeField] GameObject _debugTextPrefab;
    GameObject _debugTextContainer;

    Dungeon CurrentDungeon;

    [SerializeField] GameObject _debugRangeObject;

    void Start()
    {
        BeginDungeonDebug();
    }

    void BeginDungeonDebug()
    {
        _debugTextContainer      = new GameObject();
        _debugTextContainer.name = "Debug Text";
        DisplayDungeonDebug();  
    }

    void DisplayDungeonDebug()
    {
        foreach (Transform child in _debugTextContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        if (!_debugEnabled)
            return;

        for (int x = 0; x < CurrentDungeon.Width; x++)
        {
            for (int y = 0; y < CurrentDungeon.Height; y++)
            {
                GameObject debugText       = Instantiate(_debugTextPrefab, new Vector3(x, y, 0), Quaternion.identity);
                debugText.transform.parent = _debugTextContainer.transform;

                TextMesh textMesh = debugText.GetComponent<TextMesh>();
                HopTile thisTile  = CurrentDungeon.Tiles[x, y];

                textMesh.text     = $"W: {thisTile.IsWalkable}\nE: {thisTile.HasEntity}";
            }
        }
    }

    void DisplayValidTiles()
    {
        List<Vector3Int> validPositions = BFS.CalculateValidPositions(CurrentDungeon.Player.WorldPosition, 1, 3, CurrentDungeon);
        
        foreach (Vector3Int position in validPositions)
        {
            HopTile tileAtPosition = CurrentDungeon.GetTile(position.x, position.y);
            
            if (tileAtPosition.IsWalkable)
            {
                GameObject debugRangeObject = Instantiate(_debugRangeObject, position, Quaternion.identity);
            }
        }
    }
}
}