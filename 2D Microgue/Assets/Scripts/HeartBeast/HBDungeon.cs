using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

// CREDIT to HeartBeast for the basis of this algorith https://www.youtube.com/watch?v=K7sKUuSd8ME

namespace HB {
public class HBDungeon : MonoBehaviour // TODO Find way to not have this be a Monobehavior?
{
    public static event Action dungeonComplete;
    public static event Action newDungeon;

    [SerializeField] Tilemap tilemap;
    public Tilemap Tilemap => tilemap;
    [SerializeField] List<TileBase> tilePrefabs;
    [SerializeField] int tileCount = 80;

    // Directions
    List<Vector3Int> directions = new List<Vector3Int>() { Vector3Int.up, Vector3Int.right, Vector3Int.down, Vector3Int.left };
    Vector3Int currentDirection = Vector3Int.right;
    Vector3Int currentPosition = Vector3Int.zero;

    // Map will include 16 x 10
    [SerializeField] int worldWidth = 18;
    [SerializeField] int worldHeight = 12;
    public int WorldWidth => worldWidth;
    public int WorldHeight => worldHeight;

    List<Vector3Int> walkablePositions = new List<Vector3Int>(); // NOTE Use list if needing to look up positions by index
    public List<Vector3Int> WalkablePositions => walkablePositions;

    List<Vector3Int> deadEndPositions = new List<Vector3Int>(); // NOTE Use list if needing to look up positions by index
    public List<Vector3Int> DeadEndPositions => deadEndPositions;

    /*
    Dictionary<Vector3, HBTile> tileDictionary = new Dictionary<Vector3, HBTile>(); // NOTE Use dictionary if needing to look up position already exists
    public Dictionary<Vector3, HBTile> TileDictionary => tileDictionary;
    */

    Hashtable tileDictionary = new Hashtable(); // NOTE Use dictionary if needing to look up position already exists
    public Hashtable TileDictionary => tileDictionary;

    enum HBTileType { empty, floor, wall, deadEnd, start, exit, enemy, midPoint }
    HBTileType tileType;

    int currentCorridorLength = 0; // Used for preventing too long of hallways
    [SerializeField] int maxCorridorLength = 4;
    [SerializeField] [Range(0.01f, 0.99f)] float turnChance = 0.5f;

    public static HBDungeon instance;
    public static HBDungeon Instance => instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void OnEnable()
    {
        HBEntityHealth.PositionOfDeath += SetTileTypeToFloor;
        // HBTile.ExitTileTriggered       += GenerateDungeon; // ! Delete this after testing
        HBTileExit.ExitTileTriggered       += GenerateDungeon;
    }

    void OnDisable()
    {
        HBEntityHealth.PositionOfDeath -= SetTileTypeToFloor;
    }

    void Start()
    {
        Vector3Int randomWorldPosition = GetRandomWorldPosition();

        GenerateDungeon(randomWorldPosition);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Vector3Int randomWorldPosition = GetRandomWorldPosition();

            GenerateDungeon(randomWorldPosition); 
        }
    }

    Vector3Int GetRandomWorldPosition()
    {
        return new Vector3Int(Random.Range(1, worldWidth - 1), Random.Range(1, worldHeight - 1), 0);
    }

    public void GenerateDungeon(Vector3Int _startPosition)
    {
        BroadcastNewDungeon();
        ClearMap();
        FloodfillMap((int)HBTileType.wall);
        MarkStartTile(_startPosition);
        GeneratePathways(_startPosition, tileCount);
        MarkExitTile();
        MarkDeadEnds();
        BroadcastDungeonComplete();
    }

    void ClearMap()
    {
        tilemap.ClearAllTiles();
        tileDictionary.Clear();
        walkablePositions.Clear();
    }

    void FloodfillMap(int _tileTypeIndex)
    {
        Vector3Int tilePosition;

        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                tilePosition = new Vector3Int(x, y, 0);
                // AddNewTile(tilePosition, tilemap, _tileTypeIndex); // ! Delete this after testing
                HBTileWall wallTile = new HBTileWall(currentPosition, tilemap);
                int tilePrefabIndex = (int)HBTileType.wall;
                AddNewTile(tilePosition, tilemap, tilePrefabIndex, wallTile);
                
            }
        }
    }

    void MarkStartTile(Vector3Int _startingPosition)
    {
        currentPosition = _startingPosition;

        // AddNewTile(currentPosition, tilemap, (int)HBTileType.start); // ! Delete this after testing
        HBTileFloor startTile = new HBTileFloor(currentPosition, tilemap);
        int tilePrefabIndex = (int)HBTileType.start;
        AddNewTile(currentPosition, tilemap, tilePrefabIndex, startTile);
    }

    void GeneratePathways(Vector3Int _startingPosition, int _tileLimit) // Was WalkUntilFilled
    {
        while(walkablePositions.Count < _tileLimit)
        // for (int i = 0; i < 80; i++)
        {
            Vector3Int targetPosition     = currentPosition + currentDirection;
            bool maxCorridorLengthReached = currentCorridorLength >= maxCorridorLength;
            bool changeDirectionChance    = Random.value <= turnChance;
            bool targetOutsideBounds      = !HBHelper.WithinBounds(targetPosition, 0, worldWidth - 1, 0, worldHeight - 1);

            if (maxCorridorLengthReached && changeDirectionChance || targetOutsideBounds) { ChangeDirection(); }

            else
            {
                currentCorridorLength += 1;
                currentPosition       = targetPosition;

                if (!walkablePositions.Contains(currentPosition)) 
                { 
                    // AddNewTile(currentPosition, tilemap, (int)HBTileType.floor); // ! Delete after testing
                    HBTileFloor floorTile = new HBTileFloor(currentPosition, tilemap);
                    int tilePrefabIndex = (int)HBTileType.floor;
                    AddNewTile(currentPosition, tilemap, tilePrefabIndex, floorTile);
                }
            }
        }
    }

    void ChangeDirection()
    {
        currentCorridorLength = 0;

        List<Vector3Int> availableDirections = new List<Vector3Int>(directions);
        availableDirections.Remove(currentDirection);
        availableDirections = HBHelper.ShuffleList(availableDirections);
        currentDirection    = availableDirections[0];
        availableDirections.RemoveAt(0);

        while (!HBHelper.WithinBounds(currentPosition + currentDirection, 0, worldWidth - 1, 0, worldHeight - 1))
        {
            currentDirection = availableDirections[0];
            availableDirections.RemoveAt(0);
        }
    }

    void AddNewTile(Vector3Int _tilePosition, Tilemap _tilemap, int _tilePrefabIndex, HBTile _tileType)
    {
        _tilemap.SetTile(_tilePosition, tilePrefabs[_tilePrefabIndex]);

        _tileType.tilemap.SetTile(_tileType.localPosition, _tileType.tileBase);

        HBTile newTile = _tileType;
        newTile.localPosition = _tilePosition;
        newTile.tilemap       = _tilemap;

        if(tileDictionary.ContainsKey(_tilePosition))
        {
            tileDictionary.Remove(_tilePosition); // TODO May need to change this to overwrite dictionary entries
            if (newTile is HBTileFloor) walkablePositions.Remove(_tilePosition);
        }

        tileDictionary.Add(_tilePosition, newTile);
        if (newTile is HBTileFloor) walkablePositions.Add(_tilePosition);
    }

    void AddNewTileOld(Vector3Int _tilePosition, Tilemap _tilemap, int _tileTypeIndex)
    {
        /*
        _tilemap.SetTile(_tilePosition, tilePrefabs[_tileTypeIndex]);

        var newTile = new HBTile
        {
            localPosition = _tilePosition,
            tilemap       = _tilemap,
            tileType      = (HBTile.HBTileType)_tileTypeIndex
        };

        if(tileDictionary.TryGetValue(_tilePosition, out HBTile alreadyExistingTile))
        {
            tileDictionary.Remove(_tilePosition); // TODO May need to change this to overwrite dictionary entries
            if (newTile.IsWalkable) walkablePositions.Remove(_tilePosition);
        }

        tileDictionary.Add(_tilePosition, newTile);
        if (newTile.IsWalkable) walkablePositions.Add(_tilePosition);
        */
    }

    void MarkExitTile()
    {
        Vector3Int startPosition = walkablePositions[0];
        Vector3Int exitPosition  = GetFurthestPosition(startPosition);

        // AddNewTile(exitPosition, tilemap, (int)HBTileType.exit); // ! Delete after testing
        HBTileFloor exitTile = new HBTileFloor(currentPosition, tilemap);
        int tilePrefabIndex = (int)HBTileType.exit;
        AddNewTile(exitPosition, tilemap, tilePrefabIndex, exitTile);
    }

    Vector3Int GetFurthestPosition(Vector3Int _startPosition)
    {
        Vector3Int exitPosition  = walkablePositions[walkablePositions.Count - 1];
        int exitPositionIndex    = walkablePositions.Count - 1;

        for (int i = 0; i < walkablePositions.Count; i++)
        {
            Vector3Int thisPosition = walkablePositions[i];

            if (Vector3Int.Distance(thisPosition, _startPosition) > Vector3Int.Distance(exitPosition, _startPosition))
            {
                exitPosition = thisPosition;
                exitPositionIndex = i;
            }
        }

        return exitPosition;
    }

    void MarkDeadEnds()
    {
        List<Vector3Int> availableDirections = new List<Vector3Int>(directions);

        for (int i = 0; i < walkablePositions.Count; i++)
        {
            Vector3Int thisPosition = walkablePositions[i];

            int neighborCount = 0;

            for (int j = 0; j < availableDirections.Count; j++)
            {
                Vector3Int adjacentPosition = thisPosition + availableDirections[j];

                if (walkablePositions.Contains(adjacentPosition))
                {
                    neighborCount++;
                }
            }

            if (neighborCount == 1 && !deadEndPositions.Contains(thisPosition))
            {
                // AddNewTile(thisPosition, tilemap, (int)HBTileType.deadEnd); // ! Delete after testing
                HBTileFloor deadEndTile = new HBTileFloor(currentPosition, tilemap);
                int tilePrefabIndex = (int)HBTileType.deadEnd;
                AddNewTile(thisPosition, tilemap, tilePrefabIndex, deadEndTile);
                deadEndPositions.Add(thisPosition);
            }
        }
    }

    public void SetTileTypeToFloor(Vector3 _tilePosition) // ! Delete this if no longer needed
    {
        // HBTile tileToSet = tileDictionary[_tilePosition];

        // tileToSet.tileType = HBTile.HBTileType.floor;
    }

    void BroadcastNewDungeon() { newDungeon?.Invoke(); }

    void BroadcastDungeonComplete() { dungeonComplete?.Invoke(); }
}
}