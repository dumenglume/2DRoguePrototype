using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// CREDIT to HeartBeast for the basis of this algorith https://www.youtube.com/watch?v=K7sKUuSd8ME
public class HBDungeonOld : MonoBehaviour
{
    /*
    [SerializeField] Tilemap tilemapFloor;
    [SerializeField] Tilemap tilemapWalls;
    [SerializeField] List<TileBase> tilePrefabs;
    [SerializeField] int enemyTileCount = 10;

    List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    Vector2Int currentPosition = Vector2Int.zero; // TODO Switch to dictionary
    Vector2Int currentDirection = Vector2Int.right;

    [SerializeField] int worldWidth = 18;
    [SerializeField] int worldHeight = 12;

    [SerializeField] int tileCount = 100;
    List<Vector2Int> tilePositions = new List<Vector2Int>(); // Previously "stepHistory"

    public enum TileType { empty, floor, wall, deadEnd, start, exit, enemy, midPoint }
    List<TileType> tileTypes = new List<TileType>();

    Dictionary<Vector2Int, TileType> tileDictionary = new Dictionary<Vector2Int, TileType>(); // EXPERIMENTAL 


    int stepsSinceDirectionChange = 0; // Used for preventing too long of hallways
    [SerializeField] int maxCorridorLength = 4;
    [SerializeField] [Range(0.0f, 0.5f)] float turnChance = 0.25f;

    [SerializeField] bool spawnRoomOnTurn = true;
    [SerializeField] Vector2Int roomWidthRange = new Vector2Int(2, 4);
    [SerializeField] Vector2Int roomHeightRange = new Vector2Int(2, 4);


    public static HBDungeonOld instance;
    public static HBDungeonOld Instance => instance;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        int worldCenterX = Mathf.FloorToInt((worldWidth + 1) * 0.5f);
        int worldCenterY = Mathf.FloorToInt((worldHeight + 1) * 0.5f);
        Vector2Int worldCenter = new Vector2Int(worldCenterX, worldCenterY);

        Initialize(new Vector2Int(Random.Range(1, worldWidth - 1), Random.Range(1, worldHeight - 1)));
        WalkUntilFilled(tileCount);
        SpawnLevel();
        //PlaceTiles();
    }

    void Initialize(Vector2Int _startingPosition)
    {
        tilePositions.Clear();
        tileTypes.Clear();

        currentPosition = _startingPosition;
        tilePositions.Add(currentPosition);
        tileTypes.Add(TileType.start);

        tileDictionary.Add(currentPosition, TileType.start);
    }

    void WalkUntilFilled(int _totalSteps)
    {
        while(tilePositions.Count < _totalSteps)
        {
            if (stepsSinceDirectionChange >= maxCorridorLength && Random.value <= turnChance || !SpaceAvailable(currentPosition + currentDirection, true)) // NOTE Change && to || for different results
            {
                ChangeDirection();
            }
        }

        // MarkMidTile();
        MarkExitTile();
    }

    bool SpaceAvailable(Vector2Int _position, bool countSteps)
    {
        Vector2Int thisPosition = _position;

        if (!WithinBounds(thisPosition)) { return false; }

        if (countSteps) { stepsSinceDirectionChange += 1; }

        currentPosition = thisPosition;

        if (!tilePositions.Contains(currentPosition)) { AddNewSpace(); } // Will allow for crossing over existing spaces to ensure desired space amount is always reached

        return true;
    }

    void AddNewSpace()
    {
        tilePositions.Add(currentPosition); // TODO Switch to dictionary that adds position and tileType
        TileType thisSpace = GetSpaceType(); // TODO Go back and implement rooms
        tileTypes.Add(thisSpace);

        tileDictionary.Add(currentPosition, thisSpace);
    }

    bool SpaceAvailableOld()
    {
        Vector2Int thisPosition = currentPosition + currentDirection;

        if (!WithinBounds(thisPosition)) { return false; }

        stepsSinceDirectionChange += 1;
        currentPosition = thisPosition;

        if (!tilePositions.Contains(currentPosition))
        {
            tilePositions.Add(currentPosition);

            TileType thisSpace = GetSpaceType();
            tileTypes.Add(thisSpace);
        }

        return true;
    }

    TileType GetSpaceType()
    {
        TileType thisSpaceType = TileType.empty;

        int uniform = Mathf.FloorToInt(tileCount / enemyTileCount);

        if (tilePositions.Count % uniform == 0) // TODO Change to switch statement or better alternative
        {
            thisSpaceType = TileType.enemy;
        }

        else
        {
            thisSpaceType = TileType.floor;
        }

        return thisSpaceType;
    }

    void MarkExitTile()
    {
        Vector2Int startPosition = tilePositions[0];
        Vector2Int exitPosition  = tilePositions[tilePositions.Count - 1];
        int exitPositionIndex    = tilePositions.Count - 1;

        for (int i = 0; i < tilePositions.Count; i++)
        {
            Vector2Int thisPosition = tilePositions[i];

            if (Vector2Int.Distance(thisPosition, startPosition) > Vector2Int.Distance(exitPosition, startPosition))
            {
                exitPosition = thisPosition;
                exitPositionIndex = i;
            }
        }

        tileTypes[exitPositionIndex] = TileType.exit; // TODO May need to change if it removes an enemy
        tileDictionary[exitPosition] = TileType.exit;
    }

    void MarkMidTile()
    {
        Vector2Int startPosition = tilePositions[0];
        Vector2Int midPosition   = Vector2Int.RoundToInt(tilePositions[tilePositions.Count / 2]);
        int midPositionIndex     = Mathf.RoundToInt(tilePositions.Count / 2);

        for (int i = 0; i < tilePositions.Count; i++)
        {
            Vector2Int thisPosition = tilePositions[i];

            if (Vector2Int.Distance(thisPosition, startPosition) > Vector2Int.Distance(midPosition, startPosition))
            {
                midPosition = thisPosition;
                midPositionIndex = i;
            }
        }
        
        print(midPosition + " " + midPositionIndex);

        tileTypes[midPositionIndex] = TileType.midPoint; // TODO May need to change if it removes an enemy
    }

    void ChangeDirection()
    {
        // if (spawnRoomOnTurn) { CreateRoom(position); }

        stepsSinceDirectionChange = 0;

        List<Vector2Int> availableDirections = new List<Vector2Int>(directions);
        availableDirections.Remove(currentDirection);
        availableDirections = HBHelper.ShuffleList(availableDirections);

        currentDirection = availableDirections[0];
        availableDirections.RemoveAt(0);

        while (!WithinBounds(currentPosition + currentDirection))
        {
            currentDirection = availableDirections[0];
            availableDirections.RemoveAt(0);
        }
    }

    void CreateRoom(Vector2Int _position)
    {
        int roomWidth  = Random.Range(roomWidthRange.x, roomWidthRange.y);
        int roomHeight = Random.Range(roomHeightRange.x, roomHeightRange.y);

        Vector2Int roomSize      = new Vector2Int(roomWidth, roomHeight);
        Vector2Int topLeftCorner = Vector2Int.RoundToInt(_position - roomSize / 2);
        print(roomSize);
        print(topLeftCorner);

        for (int y = 0; y < roomHeight; y++)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                Vector2Int newStep = topLeftCorner + new Vector2Int(x, y);

                if (!SpaceAvailable(newStep, false)) { return; }


            }
        }

        print("Room placed at " + _position);
    }

    void CreateRoomOld(Vector2Int _position)
    {
        int roomWidth  = Random.Range(roomWidthRange.x, roomWidthRange.y);
        int roomHeight = Random.Range(roomHeightRange.x, roomHeightRange.y);

        Vector2Int roomSize      = new Vector2Int(roomWidth, roomHeight);
        Vector2Int topLeftCorner = Vector2Int.RoundToInt(_position - roomSize / 2);
        print(roomSize);
        print(topLeftCorner);

        for (int y = 0; y < roomHeight; y++)
        {
            for (int x = 0; x < roomWidth; x++)
            {
                Vector2Int newStep = topLeftCorner + new Vector2Int(x, y);

                if (!WithinBounds(newStep)) { return; }

                if (tilePositions.Contains(currentPosition)) { return; }
                    
                tilePositions.Add(currentPosition); 
            }
        }

        print("Room placed at " + _position);
    }

    void SpawnLevel()
    {
        // Flood fill grid with walls
        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                SpawnTiles(x, y, tilemapWalls, tilePrefabs[2]);
            }
        }

        // Carve out floors
        for (int i = 0; i < tilePositions.Count; i++)
        {
            Vector2Int thisSpace = tilePositions[i];

            SpawnTiles(thisSpace.x, thisSpace.y, tilemapWalls, null); // Carve out walls so floor will appear
            SpawnTiles(thisSpace.x, thisSpace.y, tilemapFloor, tilePrefabs[(int)tileTypes[i]]);
        }
    }

    void SpawnTiles(int _x, int _y, Tilemap _tilemap, TileBase _tile)
    {
        Vector3Int tilePosition = new Vector3Int(_x, _y, 0);
        _tilemap.SetTile(tilePosition, _tile);
    }

    bool WithinBounds(Vector2Int _position)
    {
        return _position.x > 0 && _position.x < worldWidth - 1 && _position.y > 0 && _position.y < worldHeight - 1;
    }

    /*
    void PlaceTiles()
    {
        int uniform = Mathf.FloorToInt(spaceCount / enemyTileCount);

        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                TileBase wallTile = tileTypes[2];
                tilemap.SetTile(tilePosition, wallTile);
            }
        }

        for (int i = 0; i < stepHistory.Count; i++)
        {
            Vector2Int thisStep = stepHistory[i];
            Vector3Int tilePosition = new Vector3Int(thisStep.x, thisStep.y, 0);

            TileBase tileToSpawn = GetTileType(i);
            tilemap.SetTile(tilePosition, tileToSpawn);
        }
    }
    */
}