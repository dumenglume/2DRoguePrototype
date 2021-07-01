using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] int playerPowerLevel = 1;
    public int PlayerPowerLevel => playerPowerLevel;

    Dungeon dungeon;

    void OnEnable()
    {
        TilemapFog.fogTileRevealed += OnFogReveal;
        PlayerMovement.roomBeingEntered += PlayerEnteredRoom;
    }

    void OnDisable()
    {
        TilemapFog.fogTileRevealed -= OnFogReveal;
        PlayerMovement.roomBeingEntered -= PlayerEnteredRoom;
    }

    void OnFogReveal()
    {
        
    }

    void PlayerEnteredRoom(Room _room)
    {
        if (_room.roomType == Room.RoomType.combat)
        {
            Debug.Log("Player conquered this fear but aquired 3 stress!");

            DisableEntityInsideRoom(_room);
            _room.roomType = Room.RoomType.normal;
        }
    }

    void DisableEntityInsideRoom(Room _room)
    {
        // Turn EntitySpawner into Singleton
        // Tell EntitySpawner to disable or remove a specific entity
    }
}
