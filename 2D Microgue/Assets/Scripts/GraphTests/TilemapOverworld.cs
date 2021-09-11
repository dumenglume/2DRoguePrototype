using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Room = GraphTest.Room;

namespace GraphTest
{
public class TilemapOverworld : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] Tilemap tilemap;
    [SerializeField] CustomTile customTile;
    [SerializeField] List<CustomTile> customTileList = new List<CustomTile>();

    public static Action<Room> allTilesSpawned;

    Dungeon dungeon;

    void OnEnable() 
    {
        DungeonGeneratorV2.dungeonComplete += SpawnTiles;
        DungeonGeneratorV2.newDungeon += ClearTiles;
    }

    void OnDisable() 
    {
        DungeonGeneratorV2.dungeonComplete -= SpawnTiles;
        DungeonGeneratorV2.newDungeon -= ClearTiles;
    }

    void SpawnTiles()
    {
        dungeon = DungeonGeneratorV2.Instance.Dungeon; // TODO Switch this to interface

        for (int i = 0; i < dungeon.Rooms.Count; i++)
        {
            Room thisRoom = dungeon.Rooms[i];

            Vector3Int gridPosition = grid.WorldToCell((Vector3Int) thisRoom.roomPosition);

            customTile.SetTileType((int)thisRoom.roomType);
            // customTile.SetSpriteOrientation(thisRoom.roomOrientation); // ! This is because for whatever reason using namespaces doesn't work with this
            tilemap.SetTile(gridPosition, customTile);
            customTileList.Add(customTile);
        }
    }

    void ClearTiles()
    {
        for (int i = customTileList.Count - 1; i >= 0 ; i--)
        {
            customTileList.RemoveAt(i);
        }
            
        tilemap.ClearAllTiles();
    }

    void BroadcastAllTilesSpawned()
    {
        Room spawnRoom = dungeon.Rooms[0];
        allTilesSpawned?.Invoke(spawnRoom);
    }
}
}