using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    Dungeon dungeon;

    [SerializeField] bool spawnDoorways = false;

    [SerializeField] GameObject player;
    [SerializeField] GameObject exit;
    [SerializeField] GameObject normal;
    [SerializeField] GameObject monster;
    [SerializeField] GameObject doorway;
    [SerializeField] GameObject corridor;

    List<GameObject> allEntities = new List<GameObject>();
    public List<GameObject> AllEntities { get; set; }

    /// <summary>
    /// Triggers subsequent tilemaps such as fog tilemap since entities will need to be present for later tilemaps to function properly.
    /// </summary>
    public static Action allEntitiesSpawned;

    void OnEnable() 
    {
        DungeonGenerator.dungeonComplete += InitiateSpawning;
        DungeonGenerator.newDungeon += RemoveAllEntities;
    }

    void OnDisable() 
    {
        DungeonGenerator.dungeonComplete -= InitiateSpawning;
        DungeonGenerator.newDungeon -= RemoveAllEntities;
    }

    void Start()
    {
        InitiateSpawning();
    }

    void InitiateSpawning()
    {
        dungeon = DungeonGenerator.Instance.Dungeon;

        if (dungeon != null)
        {
            for (int i = 0; i < dungeon.Rooms.Count; i++)
            {
                Room thisRoom = dungeon.Rooms[i];
                if (spawnDoorways) { SpawnDoorways(thisRoom); }
                SpawnEntities(thisRoom);
            }
        }

        BroadcastAllEntitiesSpawned();
    }

    void SpawnEntities(Room _room) // TODO Switch to Scriptable Objects
    {
        GameObject entityToSpawn;

        switch(_room.roomType)
        {
            case Room.RoomType.spawn:
                { entityToSpawn = player; }
                break;

            case Room.RoomType.exit:
                { entityToSpawn = exit; }
                break;

            case Room.RoomType.combat:
                { entityToSpawn = monster; }
                break;

            case Room.RoomType.corridor:
                { entityToSpawn = corridor; }
                break;

            default:
                { entityToSpawn = normal; }
                break;
        }

        GameObject entityInstance = Instantiate(entityToSpawn, (Vector3Int) _room.roomPosition, Quaternion.identity) as GameObject;
        allEntities.Add(entityInstance);
    }

    void BroadcastAllEntitiesSpawned()
    {
        allEntitiesSpawned?.Invoke();
    }

    void SpawnDoorways(Room _room) // TODO Find better name for this
    {
        Quaternion doorwayRotation = Quaternion.identity;

        if (_room.hasExitNorth) 
        { 
            doorwayRotation = Quaternion.Euler(0f, 0f ,0f);
            SpawnDoor(_room, doorwayRotation);
        }
        if (_room.hasExitEast)  
        { 
            doorwayRotation = Quaternion.Euler(0f, 0f ,-90f);
            SpawnDoor(_room, doorwayRotation);
        }
        if (_room.hasExitSouth) 
        { 
            doorwayRotation = Quaternion.Euler(0f, 0f ,-180f);
            SpawnDoor(_room, doorwayRotation);
        }
        if (_room.hasExitWest)  
        { 
            doorwayRotation = Quaternion.Euler(0f, 0f ,-270f);
            SpawnDoor(_room, doorwayRotation);
        }
    }

    void SpawnDoor(Room _room, Quaternion _doorwayRotation) // TODO Find better name for this too
    {
        GameObject doorwayInstance = Instantiate(doorway, (Vector3Int) _room.roomPosition, _doorwayRotation) as GameObject;
        allEntities.Add(doorwayInstance);
    }

    void RemoveAllEntities()
    {
        for (int i = allEntities.Count - 1; i >= 0 ; i--)
        {
            GameObject entityToRemove = allEntities[i];
            Destroy(entityToRemove);
            allEntities.RemoveAt(i);
        }
    }

    public void DisableEntity()
    {

    }
}
