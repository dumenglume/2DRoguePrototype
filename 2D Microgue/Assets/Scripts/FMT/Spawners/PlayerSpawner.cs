using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class PlayerSpawner : SpawnerBase
{
    [SerializeField] GameObject playerPrefab;

    protected override IEnumerator Co_SpawnObjects()
    {
        int startingPositionIndex = 0;

        _Tile tileToSpawnOn   = spawnablePositions[startingPositionIndex];

        GameObject thisObject = Instantiate(playerPrefab, tileToSpawnOn.worldPosition, Quaternion.identity); // TODO May need to move to own function

        AddObjectToDictionary(tileToSpawnOn, thisObject);
        RemoveSpawnablePosition(startingPositionIndex);

        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
    }
}
}