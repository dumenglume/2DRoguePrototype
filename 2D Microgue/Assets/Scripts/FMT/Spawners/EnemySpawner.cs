using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class EnemySpawner : SpawnerBase
{
    [SerializeField] bool markTilesAsOccupied;

    protected override IEnumerator Co_SpawnObjects()
    {
        int bufferBetweenObjects = spawnablePositions.Count / maxObjects;

        Random random = new Random();

        for (int i = 0; i < maxObjects; i++)
        {
            int buffer = i * bufferBetweenObjects;
            int randomIndex = Random.Range(buffer, buffer + bufferBetweenObjects);

            if (randomIndex >= spawnablePositions.Count)
            {
                randomIndex = spawnablePositions.Count - 1;
            }

            SpawnRandomObject(randomIndex, markTilesAsOccupied);

            if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
        }
    }

    void SpawnRandomObject(int spawnablePositionIndex, bool markThisTileAsOccupied)
    {
        _Tile tileToSpawnOn   = spawnablePositions[spawnablePositionIndex];
        int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

        GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
        tileToSpawnOn.boundGameObject = thisObject;

        if (markThisTileAsOccupied) { tileToSpawnOn.MarkAsOccupied(true); }

        AddObjectToDictionary(tileToSpawnOn, thisObject);
        RemoveSpawnablePosition(spawnablePositionIndex);
    }
}
}