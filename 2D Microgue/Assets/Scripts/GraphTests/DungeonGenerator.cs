using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GraphTest
{
public class DungeonGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] Vector2Int roomLimitRange = new Vector2Int(5, 8);
    
    [SerializeField] Vector2 corridorTurnChanceRange = new Vector2(0.1f, 0.5f);
    [SerializeField] [Range(0.0f, 1.0f)] float corridorTurnChance = 0.15f;
    
    [SerializeField] int roomDistance = 2;
    public int RoomDistance => roomDistance;

    [Header("Corridor Settings")]
    [SerializeField] float corridorLoopChance = 50f;

    [Header("Dungeon Pieces")]
    [SerializeField] List<Room> dungeonRooms;
    [SerializeField] List<Corridor> dungeonCorridors;
    List<Room> DungeonRooms => dungeonRooms;
    List<Corridor> DungeonCorridors => dungeonCorridors;

    // Actions
    public static Action newDungeon;
    public static Action dungeonComplete;

    // Dungeon Generator Singleton
    public static DungeonGenerator instance;
    public static DungeonGenerator Instance => instance;

    // Dungeon
    Dungeon dungeon;
    public Dungeon Dungeon => dungeon;

    int dungeonRoomLimit = 1; // Set to 1 to ensure spawn room is always spawned
    public int DungeonRoomLimit => dungeonRoomLimit;

    Vector2Int currentDirection = Vector2Int.zero; // Used for affecting how much path is likely to turn
    Vector2Int[] possibleDirections = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

    # region Dungeon Generation

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        dungeonRooms = new List<Room>();
        dungeonCorridors = new List<Corridor>();

        InitializeDungeon();
    }

    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            BroadcastNewDungeon();
            ClearRoomsAndCorridors();
            RandomizeDungeonSettings();
            InitializeDungeon();
        }
    }

    void BroadcastNewDungeon()
    {
        newDungeon?.Invoke();
    }

    void BroadcastDungeonComplete()
    {
        dungeonComplete?.Invoke();
    }

    void ClearRoomsAndCorridors()
    {
      List<Room> dungeonRooms = dungeon.Rooms;
      List<Corridor> dungeonCorridors = dungeon.Corridors;

      if (dungeonRooms.Count > 0) dungeon.Rooms.Clear();
      if (dungeonCorridors.Count > 0) dungeon.Corridors.Clear();
    }

    void RandomizeDungeonSettings()
    {
        // TODO Randomize room limit
        corridorTurnChance = Random.Range(corridorTurnChanceRange.x, corridorTurnChanceRange.y);
    }

    /// <summary>
    /// Generates initial spawn room and initiates first branch of the dungeon.
    /// </summary>
    void InitializeDungeon()
    {
        dungeonRoomLimit = Random.Range(roomLimitRange.x, roomLimitRange.y);

        Room spawnRoom = new Room() { roomPosition = Vector2Int.zero, roomType = Room.RoomType.spawn }; // Spawn first room at world origin

        dungeon = new Dungeon() { Rooms = { spawnRoom } }; // Always starts dungeon with spawn room

        if (dungeonRoomLimit > 1) { GenerateNewDungeon(spawnRoom); }
        else { Debug.Log("At least one room must exist for dungeon to generate."); }
    }

    /// <summary>
    /// Master method for dungeon generation
    /// </summary>
    void GenerateNewDungeon(Room _originRoom)
    {
        GenerateDungeonBranch(_originRoom);
        DungeonBitmaskRooms.BitmaskRooms(dungeon);
        Room finalRoom = dungeon.Rooms[dungeon.Rooms.Count - 1];
        MarkFinalRoom(finalRoom);
        UpdateRoomList();
        UpdateCorridorList();
        BroadcastDungeonComplete();
    }

    /// <summary>
    /// Generates a new series of rooms that will continue until the room limit is reached or it overlaps with itself.
    /// </summary>
    void GenerateDungeonBranch(Room _roomToSpawnFrom)
    {
        Room previouslySpawnedRoom = _roomToSpawnFrom;

        while(dungeon.Rooms.Count < dungeonRoomLimit)
        {
            // Get random direction to exit from and generate doorway from previous room at that exit (N, E, S, or W)

            Vector2Int directionOfNextRoom = GetNewDirection(currentDirection);
            GenerateDoorway(previouslySpawnedRoom, directionOfNextRoom);

            // Check if next room position is available
            Vector2Int directionWithDistance = directionOfNextRoom * roomDistance;
            Vector2Int nextRoomPosition = previouslySpawnedRoom.roomPosition + directionWithDistance;

            Room alreadyExistingRoom = CheckIfRoomAlreadyExists(nextRoomPosition);

            // If room already exists...
            if (alreadyExistingRoom != null)
            {
                // Generate corridor to that room
                GenerateCorridor(previouslySpawnedRoom, alreadyExistingRoom);

                if (Random.value < corridorLoopChance) // TODO Switch to percentage chance
                {
                    GenerateDoorway(alreadyExistingRoom, -directionOfNextRoom);
                }

                // TODO Assign end of branch as a special room (key, shrine, treasure, etc.)
                // Generate new branch from random room
                Room newRoomToSpawnFrom = GetRandomRoomLocation();
                GenerateDungeonBranch(newRoomToSpawnFrom);
                return; // TODO Change back to return
            }

            // Otherwise create the originally intended room
            else
            {
                Room newRoom = GenerateRoom(nextRoomPosition);

                GenerateCorridor(previouslySpawnedRoom, newRoom);
                GenerateDoorway(newRoom, -directionOfNextRoom);

                previouslySpawnedRoom = GetRandomRoomLocation();
                currentDirection = directionOfNextRoom;
            }
        }
    }

    # endregion

    # region Helper Methods

    /// <summary>
    /// Returns random vector direction (i.e. Vector2.up, Vector2.right, etc.) and randomizes distance
    /// </summary>
    Vector2Int GetNewDirection(Vector2Int _previousDirection)
    {
        float percentageChance = Random.value;
        int randomDirectionIndex = 0;

        if (_previousDirection == Vector2Int.zero || percentageChance < corridorTurnChance)
        {
            if (_previousDirection == Vector2Int.right || _previousDirection == Vector2Int.left)
            {
                randomDirectionIndex = Random.Range(0, 2);
            }

            else if (_previousDirection == Vector2Int.up || _previousDirection == Vector2Int.down)
            {
                randomDirectionIndex = Random.Range(2, 4);
            }
            
            return possibleDirections[randomDirectionIndex];
        }

        return _previousDirection;
    }

    Vector2Int GetNextDirectionOld(Vector2Int _previousDirection)
    {
        float percentageChance = (Random.value * 100f);

        if (_previousDirection == Vector2Int.zero || percentageChance < corridorTurnChance)
        {
            List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

            int randomDirectionIndex = Random.Range(0, 4);
            return directions[randomDirectionIndex];
        }

        return _previousDirection;
    }

    /// <summary>
    /// Loops through all rooms and checks if their positions match this position.
    /// </summary>
    Room CheckIfRoomAlreadyExists(Vector2Int _roomPosition)
    {
        for (int i = 0; i < dungeon.Rooms.Count; i++)
        {
            if (_roomPosition == dungeon.Rooms[i].roomPosition)
            {
                return dungeon.Rooms[i];
            }
        }

        return null;
    }

    /// <summary>
    /// Generates a randomly selected room at the desired position and adds it to the room list
    /// </summary>
    Room GenerateRoom(Vector2Int _thisRoomPosition)
    {
        float percentageChance = Random.value;

        Room.RoomType randomRoomType = Room.RoomType.normal;

        if (percentageChance < 0.25) { randomRoomType = Room.RoomType.combat; } // TODO Create an enum containing the percentages of different tiles to spawn
        
        Room thisRoom = new Room() { roomPosition = _thisRoomPosition, roomType = randomRoomType };

        dungeon.Rooms.Add(thisRoom);
        return thisRoom;
    }

    /// <summary>
    /// Generates a specific room at the desired position and adds it to the room list (Overload)
    /// </summary>
    Room GenerateRoom(Vector2Int _thisRoomPosition, Room.RoomType _roomType)
    {
        Room thisRoom = new Room() { roomPosition = _thisRoomPosition, roomType = _roomType };

        dungeon.Rooms.Add(thisRoom);
        return thisRoom;
    }

    /// <summary>
    /// Generates a connection between two rooms, assigns its orientation and adds it to the corridor list.
    /// </summary>
    void GenerateCorridor(Room _fromRoom, Room _toRoom)
    {
        Corridor thisCorridor = new Corridor() {fromRoom = _fromRoom, toRoom = _toRoom };
        thisCorridor.corridorOrientation = _fromRoom.roomPosition.x == _toRoom.roomPosition.x ? Corridor.CorridorOrientation.horizontal : Corridor.CorridorOrientation.vertical;
        dungeon.Corridors.Add(thisCorridor);
    }

    /// <summary>
    /// Flags an exit from the room passed in the direction passed.
    /// </summary>
    void GenerateDoorway(Room _room, Vector2Int _corridorDirection)
    {
        if (_corridorDirection == Vector2Int.up)    { _room.hasExitNorth = true; }
        else if (_corridorDirection == Vector2Int.right) { _room.hasExitEast  = true; }
        else if (_corridorDirection == Vector2Int.down)  { _room.hasExitSouth = true; }
        else if (_corridorDirection == Vector2Int.left)  { _room.hasExitWest  = true; }
    }

    /// <summary>
    /// Returns a random room from the room list.
    /// </summary>
    Room GetRandomRoomLocation()
    {
        return dungeon.Rooms[Random.Range(0, dungeon.Rooms.Count)];
    }

    /// <summary>
    /// Marks room as end for any special conditions dependent on end room.
    /// </summary>
    void MarkFinalRoom(Room _finalRoom)
    {
        _finalRoom.roomType = Room.RoomType.exit;
    }

    void UpdateRoomList()
    {
        for (int i = 0; i < dungeon.Rooms.Count; i++)
        {
            Room roomToAdd = dungeon.Rooms[i];
            dungeonRooms.Add(roomToAdd);
        }
    }

    void UpdateCorridorList()
    {
        for (int i = 0; i < dungeon.Corridors.Count; i++)
        {
            Corridor corridorToAdd = dungeon.Corridors[i];
            dungeonCorridors.Add(corridorToAdd);
        }
    }

    # endregion
}
}