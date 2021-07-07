using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class DungeonManager : MonoBehaviour
{
    public static event Action newDungeon;
    public static event Action dungeonComplete;

    [SerializeField] DungeonGenerator dungeonGenerator;
    [SerializeField] EntitySpawner    entitySpawner;
    [SerializeField] ItemSpawner      itemSpawner;

    public _Tile[,] tileGrid;
    Dictionary<Vector3Int, _Tile> dictionaryWalkableTiles;
    public Dictionary<Vector3Int, _Tile> DictionaryWalkableTiles { get => dictionaryWalkableTiles; set => dictionaryWalkableTiles = value; }

    List<_Tile> listWalkableTiles;
    public List<_Tile> ListWalkableTiles { get => listWalkableTiles; set => listWalkableTiles = value; }

    List<_Tile> listDeadEndTiles;
    public List<_Tile> ListDeadEndTiles => listDeadEndTiles;

    static DungeonManager instance;
    public static DungeonManager Instance => instance;

    [SerializeField] float generationSpeed = 0.0f;
    public float GenerationSpeed => generationSpeed;

    void Awake()
    {
        if   (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    void OnEnable() => _ExitTile.ExitTileTriggered += StartNewDungeon;

    void OnDisable() => _ExitTile.ExitTileTriggered -= StartNewDungeon;

    void Start() => StartNewDungeon(0, 0);

    void StartNewDungeon(int startPositionX, int startPositionY) // TODO Implement x, y to determine dungeon start position
    {
        StopAllCoroutines();
        StartCoroutine(Co_StartNewDungeon());
    }

    IEnumerator Co_StartNewDungeon()
    {
        BroadcastNewDungeon();
        ClearExistingDungeon();
        yield return dungeonGenerator.Co_BeginGenerationProcess();
        CloneDungeonTileGrid();
        GetWalkableTiles();
        yield return entitySpawner.Co_SpawnEntities();
        yield return itemSpawner.Co_SpawnItems();
        BroadcastDungeonComplete();
    }

    void ClearExistingDungeon()
    {
        dungeonGenerator.ClearAllTiles();
        entitySpawner.ClearAllEntities();
        itemSpawner.ClearAllItems();
    }

    void CloneDungeonTileGrid() => tileGrid = CloneTileArray(dungeonGenerator.TileGrid);

    _Tile[,] CloneTileArray(_Tile[,] tileArray)
    {
        _Tile[,] clonedTileArray = (_Tile[,]) tileArray.Clone();

        return clonedTileArray;
    }

    void GetWalkableTiles() => listWalkableTiles = new List<_Tile>(dungeonGenerator.DictionaryWalkableTiles.Values);

    void BroadcastNewDungeon() => newDungeon?.Invoke();

    void BroadcastDungeonComplete() => dungeonComplete?.Invoke();
}
}