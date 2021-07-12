using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EnemySpawner : SpawnerBase
{
    protected override IEnumerator Co_SpawnObjects()
    {
        int objectsSpawned = 0;

        while(objectsSpawned < maxObjects)
        {
            int spawnInterval = Mathf.CeilToInt(spawnablePositions.Count / maxObjects);

            for (int i = 0; i < spawnablePositions.Count; i++)
            {
                if (i % spawnInterval == 0) // TODO Fix this later so that 10 enemies will not spawn twice as many enemies
                {
                    _Tile tileToSpawnOn   = spawnablePositions[i];
                    int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

                    GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
                    tileToSpawnOn.gameObject = thisObject;
                    tileToSpawnOn.MarkAsOccupied(true);

                    AddObjectToDictionary(tileToSpawnOn, thisObject);
                    RemoveSpawnablePosition(i);

                    objectsSpawned ++;

                    Debug.Log($"Objects spawned: {objectsSpawned} / {maxObjects}");

                    if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
                }
            }
        }
    }
}
}