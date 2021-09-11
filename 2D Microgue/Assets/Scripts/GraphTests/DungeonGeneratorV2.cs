using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GraphTest
{
public class DungeonGeneratorV2 : MonoBehaviour
{
    [Header("Room Count Settings")]
    [SerializeField] Vector2Int roomLimitRange = new Vector2Int(5, 8);
    int dungeonRoomLimit = 1; // Setting to 1 ensures spawn room is always spawned
    public int DungeonRoomLimit => dungeonRoomLimit;

    [Header("Corridor Turn Settings")]
    [SerializeField] Vector2 corridorTurnChanceRange = new Vector2(0.25f, 0.75f);
    [SerializeField] [Range(0.0f, 1.0f)] float corridorTurnChance = 0.5f;

    [Header("Corridor Loop Settings")]
    [SerializeField] Vector2 corridorLoopChanceRange = new Vector2(0.25f, 0.75f);
    [SerializeField] [Range(0.0f, 1.0f)] float corridorLoopChance = 0.5f;

    [Header("Dungeon Branch Settings")]
    [SerializeField] bool randomizeBranchLocations = true;

    [Header("Room Distance Settings")]
    [SerializeField] int roomDistance = 2;
    public int RoomDistance => roomDistance;

    // Room list
    List<Room> dungeonRoomsList;
    public List<Room> DungeonRoomsList => dungeonRoomsList;
    int roomIDCount = 0; // Will assign a number to each room as it is created

    // Actions
    public static Action newDungeon;
    public static Action dungeonComplete;

    // Dungeon Generator Singleton
    public static DungeonGeneratorV2 instance;
    public static DungeonGeneratorV2 Instance => instance;

    // Public dungeon property
    Dungeon dungeon;
    public Dungeon Dungeon => dungeon;

    Vector2Int currentDirection = Vector2Int.zero; // Used for affecting how much path is likely to turn
    Vector2Int[] possibleDirections = new Vector2Int[4] { Vector2Int.up, Vector2Int.down, Vector2Int.right, Vector2Int.left };

    Room currentRoom;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;

        InitializeLists();
    }

    void Start()
    {
        GenerateNewDungeon();
    }

    void InitializeLists()
    {
        dungeonRoomsList = new List<Room>();
        roomIDCount = 0;
    }

    void GenerateNewDungeon()
    {
        RandomizeDungeonSettings();
        
        Room spawnRoom = new Room() { roomPosition = Vector2Int.zero, roomType = Room.RoomType.spawn };
        dungeon = new Dungeon() { Rooms = { spawnRoom } }; // Always starts dungeon with spawn room

        GenerateDungeonBranch(spawnRoom);
    }

    /// <summary>
    /// Generates a new series of rooms that will continue until the room limit is reached or it overlaps with itself.
    /// </summary>
    void GenerateDungeonBranch(Room _roomToSpawnFrom)
    {
        currentRoom = _roomToSpawnFrom;

        while(dungeon.Rooms.Count < dungeonRoomLimit)
        {
            // Choose new direction and apply distance to determine next room's position
            Vector2Int newDirection          = GetNewDirection(currentDirection);
            Vector2Int directionWithDistance = ApplyDistanceToVector(newDirection);
            Vector2Int nextRoomPosition      = currentRoom.roomPosition + directionWithDistance;

            if (RoomAlreadyExistsAtPosition(nextRoomPosition, out Room alreadyExistingRoom))
            {
                // Roll to check if rooms connnect 
                float percentageChance = Random.value;
                if (percentageChance < corridorLoopChance) { ConnectRooms(currentRoom, alreadyExistingRoom, newDirection); }

                // Choose random room to start a new branch from
                Room newRoomToSpawnFrom = GetRandomRoom();
                GenerateDungeonBranch(newRoomToSpawnFrom);
                return;
            }

            // Otherwise place a brand new room at this position and 
            Room newRoom = GenerateRoom(nextRoomPosition, ChooseRoomType2());
            ConnectRooms(currentRoom, newRoom, newDirection);

            // Determine if a new branch location needs to be chosen
            currentRoom = randomizeBranchLocations ? GetRandomRoom() : newRoom;
            currentDirection = newDirection;
        }

        // Perform final generation steps and announce completion
        MarkFinalRoom();
        DungeonBitmaskRooms.BitmaskRooms(dungeon);
        BroadcastDungeonCompletion();
    }

    void RandomizeDungeonSettings()
    {
        dungeonRoomLimit   = Random.Range(roomLimitRange.x, roomLimitRange.y);
        corridorTurnChance = Random.Range(corridorTurnChanceRange.x, corridorTurnChanceRange.y);
        corridorLoopChance = Random.Range(corridorLoopChanceRange.x, corridorLoopChanceRange.y);
    }

    /// <summary>
    /// Returns random vector direction (i.e. Vector2.up, Vector2.right, etc.) and randomizes distance
    /// </summary>
    Vector2Int GetNewDirection(Vector2Int _previousDirection)
    {
        float percentageChance = Random.value;
        int randomDirectionIndex = 0;

        if (_previousDirection == Vector2Int.right || _previousDirection == Vector2Int.left) { randomDirectionIndex = Random.Range(0, 2); }

        if (_previousDirection == Vector2Int.up || _previousDirection == Vector2Int.down) { randomDirectionIndex = Random.Range(2, 4); }

        if (_previousDirection == Vector2Int.zero || percentageChance < corridorTurnChance) 
        { 
            randomDirectionIndex = Random.Range(0, 4);
            return possibleDirections[randomDirectionIndex]; 
        }

        return _previousDirection;
    }

    /// <summary>
    /// Returns a Vector2Int direction multiplied by roomDistance.
    /// </summary>
    Vector2Int ApplyDistanceToVector(Vector2Int _direction)
    {
        return _direction * roomDistance;
    }

    /// <summary>
    /// Loops through all rooms and checks if their positions match this position.
    /// </summary>
    bool RoomAlreadyExistsAtPosition(Vector2Int _roomPosition, out Room existingRoom)
    {
        for (int i = 0; i < dungeon.Rooms.Count; i++)
        {
            if (_roomPosition == dungeon.Rooms[i].roomPosition)
            {
                existingRoom = dungeon.Rooms[i];
                return true;
            }
        }

        existingRoom = null;
        return false;
    }

    void ConnectRooms(Room _fromRoom, Room _toRoom, Vector2Int _directionFromTo)
    {
        GenerateCorridor(_fromRoom, _toRoom);
        GenerateDoorway(_fromRoom, _directionFromTo);
        GenerateDoorway(_toRoom, -_directionFromTo);
    }

    /// <summary>
    /// Flags an exit from the room passed in the direction passed.
    /// </summary>
    void GenerateDoorway(Room _room, Vector2Int _directionOfNextRoom)
    {
        if      (_directionOfNextRoom == Vector2Int.up)    { _room.hasExitNorth = true; }
        else if (_directionOfNextRoom == Vector2Int.right) { _room.hasExitEast  = true; }
        else if (_directionOfNextRoom == Vector2Int.down)  { _room.hasExitSouth = true; }
        else if (_directionOfNextRoom == Vector2Int.left)  { _room.hasExitWest  = true; }
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
    /// Returns a random room from the room list.
    /// </summary>
    Room GetRandomRoom()
    {
        return dungeon.Rooms[Random.Range(0, dungeon.Rooms.Count)];
    }

    /// <summary>
    /// Generates a randomly selected room at the desired position and adds it to the room list
    /// </summary>
    Room GenerateRoom(Vector2Int _thisRoomPosition, Room.RoomType _roomType)
    {
        roomIDCount++;
        Room newRoom = new Room() { roomPosition = _thisRoomPosition, roomType = _roomType, roomID = roomIDCount };
        dungeon.Rooms.Add(newRoom);
        dungeonRoomsList.Add(newRoom);
        return newRoom;
    }

    Room.RoomType ChooseRoomType()
    {
        Room.RoomType roomType = Room.RoomType.normal;
        return roomType;
    }

    Room.RoomType ChooseRoomType2() // EXPERIMENTAL 
    {
        Room.RoomType roomType;

        float percentageChance = Random.value;

        if (percentageChance < 0.2f) { roomType = Room.RoomType.combat; }

        else { roomType = Room.RoomType.normal; }        

        return roomType;
    }

    /// <summary>
    /// Marks room as end for any special conditions dependent on end room.
    /// </summary>
    void MarkFinalRoom()
    {
        Room finalRoom = dungeon.Rooms[dungeon.Rooms.Count - 1];
        finalRoom.roomType = Room.RoomType.exit;
    }

    void BroadcastDungeonCompletion()
    {
        Debug.Log("Dungeon Complete");
        dungeonComplete?.Invoke();
    }
}
}