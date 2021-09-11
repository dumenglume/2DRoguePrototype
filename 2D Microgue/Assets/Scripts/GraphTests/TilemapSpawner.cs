using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GraphTest
{
public abstract class TilemapSpawner : MonoBehaviour
{
    protected Dungeon dungeon;
    [SerializeField] protected Tilemap tilemap;
    [SerializeField] protected TileBase tile;

    protected virtual void OnEnable() 
    {
        DungeonGeneratorV2.dungeonComplete += InitiateSpawning;
        DungeonGeneratorV2.newDungeon += RemoveTiles;
    }

    protected virtual void OnDisable() 
    {
        DungeonGeneratorV2.dungeonComplete -= InitiateSpawning;
        DungeonGeneratorV2.newDungeon -= RemoveTiles;
    }

    protected virtual void InitiateSpawning()
    {
    }

    protected virtual void RemoveTiles()
    {
    }
}
}