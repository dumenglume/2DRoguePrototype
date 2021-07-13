using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class GoldSpawner : SpawnerBase
{
      protected override IEnumerator Co_SpawnObjects()
    {
        int objectsSpawned = 0;

        while(objectsSpawned < maxObjects)
        {
            int spawnInterval = Random.Range(0, spawnablePositions.Count);

            for (int i = 0; i < spawnablePositions.Count; i++)
            {
                if (i == spawnInterval)
                {
                    _Tile tileToSpawnOn   = spawnablePositions[i];
                    int randomObjectIndex = Random.Range(0, objectPrefabsList.Count);

                    GameObject thisObject = Instantiate(objectPrefabsList[randomObjectIndex], tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function
                    tileToSpawnOn.boundGameObject = thisObject;

                    AddObjectToDictionary(tileToSpawnOn, thisObject);
                    RemoveSpawnablePosition(i);

                    objectsSpawned ++;

                    if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
                }
            }
        }
    }
}
}