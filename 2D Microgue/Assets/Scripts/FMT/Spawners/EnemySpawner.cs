using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class EnemySpawner : SpawnerBase
{
    protected IEnumerator Co_SpawnObjectsOld()
    {
        int objectsSpawned = 0;

        while(objectsSpawned < maxObjects)
        {
            int spawnInterval = Mathf.CeilToInt(spawnablePositions.Count / maxObjects);

            Debug.Log($"Spawn Interval: {spawnInterval}");

            for (int i = 0; i < spawnablePositions.Count; i++)
            {
                Debug.Log($"Iteration modulo: {i % spawnInterval}");

                if (i % spawnInterval == 0) // TODO Fix this later so that 10 enemies will not spawn twice as many enemies
                {
                    _Tile tileToSpawnOn   = spawnablePositions[i];
                    int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

                    GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
                    tileToSpawnOn.boundGameObject = thisObject;
                    tileToSpawnOn.MarkAsOccupied(true);

                    AddObjectToDictionary(tileToSpawnOn, thisObject);
                    RemoveSpawnablePosition(i);

                    Debug.Log($"Enemy spawned: {thisObject.name}");

                    objectsSpawned ++;

                    Debug.Log($"Objects spawned: {objectsSpawned} / {maxObjects}");

                    if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
                }
            }
        }
    }

    void SpawnRandomObject(int spawnablePositionIndex)
    {
        _Tile tileToSpawnOn   = spawnablePositions[spawnablePositionIndex];
        int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

        GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
        tileToSpawnOn.boundGameObject = thisObject;
        tileToSpawnOn.MarkAsOccupied(true);

        AddObjectToDictionary(tileToSpawnOn, thisObject);
        RemoveSpawnablePosition(spawnablePositionIndex);

        Debug.Log($"Enemy spawned: {thisObject.name}");
    }

    protected override IEnumerator Co_SpawnObjects()
    {
        int bufferBetweenObjects = spawnablePositions.Count / maxObjects; // 5

        Random random = new Random();

        for (int i = 0; i < maxObjects; i++)
        {
            int buffer = i * bufferBetweenObjects;
            int randomIndex = Random.Range(buffer, buffer + bufferBetweenObjects);

            if (randomIndex >= spawnablePositions.Count)
            {
                randomIndex = spawnablePositions.Count - 1;
            }

            SpawnRandomObject(randomIndex);

            if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
        }
    }
}
}