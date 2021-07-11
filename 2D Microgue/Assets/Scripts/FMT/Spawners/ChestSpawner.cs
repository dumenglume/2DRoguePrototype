using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class ChestSpawner : SpawnerBase
{
    protected override void GetSpawnablePositions() => spawnablePositions = new List<_Tile>(DungeonManager.Instance.ListDeadEndTiles);

      protected override IEnumerator Co_SpawnObjects()
    {
        int objectsSpawned = 0;

        maxObjects = Mathf.Clamp(maxObjects, 0, spawnablePositions.Count); // To prevent bugs in while loop

        while(objectsSpawned < maxObjects && spawnablePositions.Count > 0)
        {
            int spawnInterval = Random.Range(0, spawnablePositions.Count);

            for (int i = 0; i < spawnablePositions.Count; i++)
            {
                if (i == spawnInterval) // TODO Fix this later so that 10 enemies will not spawn twice as many enemies
                {
                    _Tile tileToSpawnOn   = spawnablePositions[i];
                    int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

                    GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
                    tileToSpawnOn.gameObject = thisObject;

                    AddObjectToDictionary(tileToSpawnOn, thisObject);
                    RemoveSpawnablePosition(i); // ! Need to properly remove deadends from DungeonManager's walkable tile list

                    objectsSpawned ++;

                    if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
                }
            }
        }
    }
}
}