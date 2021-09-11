using System.Collections.Generic;
using UnityEngine;

namespace GraphTest
{
public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> buildingsToSpawn = new List<GameObject>();

    int spawnedBuildings = 0;

    void OnEnable() 
    {
        DungeonGeneratorV2.dungeonComplete += SpawnBuildings;
    }

    void OnDisable() 
    {
        DungeonGeneratorV2.dungeonComplete -= SpawnBuildings;
    }

    void SpawnBuildings()
    {
        List<Room> roomList = DungeonGeneratorV2.Instance.DungeonRoomsList;
        List<Vector3Int> filledPositions = new List<Vector3Int>();

        while (spawnedBuildings < buildingsToSpawn.Count)
        {
            int randomBuildingIndex = Random.Range(0, buildingsToSpawn.Count);
            int randomRoomIndex     = Random.Range(0, roomList.Count);

            Vector3Int positionToSpawn = (Vector3Int) roomList[randomRoomIndex].roomPosition;
            if (filledPositions.Contains(positionToSpawn)) { return; }

            GameObject buildingToSpawn = Instantiate(buildingsToSpawn[randomBuildingIndex], positionToSpawn, Quaternion.identity) as GameObject;
            buildingsToSpawn.RemoveAt(randomBuildingIndex);
            filledPositions.Add(positionToSpawn);
        }

        Debug.Log("All buildings spawned: " + buildingsToSpawn.Count);
    }
}
}