using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Action<Room> roomBeingEntered;

    [Tooltip("Useful for generating a traditional randomly generated room vs. an overworld style room connected by nodes")]
    [SerializeField] bool ignoreRoomConnections = false;
    [SerializeField] float movementDuration = 0.25f;
    [SerializeField] float bumpDuration = 0.25f;
    
    bool isMoving = false;
    public bool IsMoving => isMoving;

    int distanceBetweenRooms = 1;

    Dungeon dungeon;

    void Start()
    {
        dungeon = DungeonGeneratorV2.Instance.Dungeon;
        distanceBetweenRooms = DungeonGeneratorV2.Instance.RoomDistance;

        Room spawnRoom = dungeon.Rooms[0];
        roomBeingEntered?.Invoke(spawnRoom);
    }

    /// <summary>
    /// Attempts to move player to another room
    /// </summary>
    public void AttemptToMove(Vector2 _direction)
    {
        Vector2Int currentPosition         = Vector2Int.RoundToInt((Vector2) transform.position);
        Vector2Int directionWithDistance   = Vector2Int.RoundToInt(_direction * distanceBetweenRooms);
        Vector2Int destinationRoomPosition = currentPosition + directionWithDistance;

        if (!RoomMatchesPosition(destinationRoomPosition, out Room destinationRoom)) { return; }

        if (RoomHasValidDoorway(destinationRoom, _direction)) 
        {
            if (destinationRoom.roomType != Room.RoomType.combat) { MoveToRoom(destinationRoom); }

            else { BumpAgainstEntity(currentPosition, destinationRoom); }
        }
    }

    /// <summary>
    /// Loops through all rooms and returns the room matching the desired position
    /// </summary>
    bool RoomMatchesPosition(Vector2Int _destinationRoomPosition, out Room room)
    {
        for (int i = 0; i < dungeon.Rooms.Count; i++)
        {
            Room thisRoom = dungeon.Rooms[i];

            if (thisRoom.roomPosition == _destinationRoomPosition) 
            { 
                room = thisRoom;
                return true;
            }
        }

        room = null;
        return false;
    }

    /// <summary>
    /// Checks if room being entered has a valid doorway in the direction it is being entered from.
    /// </summary>
    bool RoomHasValidDoorway(Room _room, Vector2 _direction)
    {
        if (ignoreRoomConnections) { return true; } // TODO May need better implementation of this

        return _room.hasExitNorth && _direction == Vector2.down 
            || _room.hasExitEast  && _direction == Vector2.left 
            || _room.hasExitSouth && _direction == Vector2.up
            || _room.hasExitWest  && _direction == Vector2.right;
    }

    /// <summary>
    /// Triggers any actions associated with this room (combat, events, etc.)
    /// </summary>
    void MoveToRoom(Room _room)
    {
        if (LeanTween.isTweening()) { return; }

        isMoving = true;
        LeanTween.move(gameObject, _room.roomPosition, movementDuration).setEaseInOutQuad().setOnComplete(SetMovingToFalse);

        BroadcastRoomBeingEntered(_room);
    }

    void BumpAgainstEntity(Vector2Int _startingPosition, Room _destinationRoom)
    {
        if (LeanTween.isTweening()) { return; }

        isMoving = true;

        LTSeq sequence = LeanTween.sequence(); // Player will bump against entity and bump back to original spot

        // TODO Add squash and stretch animation here

        sequence.append(LeanTween.move(gameObject, _destinationRoom.roomPosition, bumpDuration).setEaseInOutQuad());

        sequence.append( () => 
        { 
            BroadcastRoomBeingAttacked(_destinationRoom); // TODO Do entity interaction here
            Debug.Log("Attacking " + _destinationRoom);
        } );

        sequence.append(LeanTween.move(gameObject, _startingPosition, bumpDuration).setEaseInOutQuad().setOnComplete(SetMovingToFalse));
    }

    void SetMovingToFalse()
    {
        isMoving = false;
    }

    void BroadcastRoomBeingEntered(Room _room) { roomBeingEntered?.Invoke(_room); }
    void BroadcastRoomBeingAttacked(Room _room) { /* roomBeingEntered?.Invoke(_room); */ }
}
