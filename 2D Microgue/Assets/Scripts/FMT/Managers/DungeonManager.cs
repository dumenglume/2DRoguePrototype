using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace FMT
{
public class DungeonManager : MonoBehaviour
{
    public static event Action newDungeon;
    public static event Action dungeonComplete;

    [SerializeField] DungeonGenerator dungeonGenerator;

    [SerializeField] List<SpawnerBase> spawners;
    [SerializeField] FogManager fogManager;

    [SerializeField] int worldWidth  = 18;
    [SerializeField] int worldHeight = 12;

    [SerializeField] Tilemap tilemap;
    public _Tile[,] tileGrid;
    Dictionary<Vector3Int, _Tile> dictionaryWalkableTiles; // Used for copying over walkable tiles from dungeon generator which are added to listWalkableTiles

    List<_Tile> listWalkableTiles; // Used for when needing to loop through only walkable tiles vs. all tiles (for performance)
    public List<_Tile> ListWalkableTiles => listWalkableTiles;

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

    void Start() => StartNewDungeon();

    void StartNewDungeon() // TODO Implement x, y to determine dungeon start position
    {
        StopAllCoroutines();
        StartCoroutine(Co_StartNewDungeon());
    }

    IEnumerator Co_StartNewDungeon()
    {
        BroadcastNewDungeon();
        ClearExistingDungeon();
        yield return dungeonGenerator.Co_BeginGenerationProcess();
        GetDungeonData();
        yield return Co_BeginSpawnProcess();
        fogManager.FloodfillFog();
        BroadcastDungeonComplete();
    }

    IEnumerator Co_BeginSpawnProcess()
    {
        for (int i = 0; i < spawners.Count; i++) 
        { 
            yield return spawners[i].Co_BeginSpawnProcess(); 
        }
    }

    void ClearExistingDungeon()
    {
        dungeonGenerator.ClearAllTiles();

        for (int i = 0; i < spawners.Count; i++) { spawners[i].ClearAllObjects(); }
    }

    void GetDungeonData() 
    { 
        worldWidth        = dungeonGenerator.WorldWidth;
        worldHeight       = dungeonGenerator.WorldHeight;
        tileGrid          = CloneTileArray(dungeonGenerator.TileGrid);
        tilemap           = dungeonGenerator.Tilemap;
        listWalkableTiles = new List<_Tile>(dungeonGenerator.DictionaryWalkableTiles.Values);
        listDeadEndTiles  = new List<_Tile>(dungeonGenerator.ListDeadEndTiles);
        Debug.Log($"Dead Ends Transferred: { listDeadEndTiles.Count }");
    }

    _Tile[,] CloneTileArray(_Tile[,] tileArray)
    {
        _Tile[,] clonedTileArray = (_Tile[,]) tileArray.Clone();

        return clonedTileArray;
    }

    public void PlaceTile(int x, int y, _Tile newTile)
    {
        newTile.SetProperties(new Vector3Int(x, y, 0), tilemap);
        newTile.DrawTile();
        tileGrid[x, y] = newTile;

        if (newTile is IAmWalkable) { AddTileToWalkableList(newTile); }
    }
    
    public _Tile GetTile (int x, int y) 
    {
        return tileGrid[x, y];
    }

    _Tile GetRandomTile()
    {
        int randomX = (int) Random.Range(1, worldWidth - 1);
        int randomY = (int) Random.Range(1, worldHeight - 1);

        _Tile randomTile = GetTile(randomX, randomY);

        return randomTile;
    }

    public void AddTileToWalkableList(_Tile newTile)
    {
        Vector3Int newTilePosition = newTile.worldPosition;
        dictionaryWalkableTiles[newTilePosition] = newTile;
    }

    public void RemoveFromWalkableList(int index)
    {
        listWalkableTiles.RemoveAt(index);
    }

    void BroadcastNewDungeon() => newDungeon?.Invoke();

    void BroadcastDungeonComplete() => dungeonComplete?.Invoke();
}
}