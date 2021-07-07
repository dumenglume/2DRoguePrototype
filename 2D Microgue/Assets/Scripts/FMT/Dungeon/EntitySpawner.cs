using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EntitySpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    List<_Tile> spawnablePositions;
    _Tile playerStartTile;

    Dictionary<_Tile, GameObject> entityDictionary;

    public IEnumerator Co_SpawnEntities()
    { 
        InitializeEntityDictionary();
        print("Entities being spawned");
        GetSpawnablePositions();
        SpawnPlayer();
        yield return new WaitForSeconds(DungeonManager.Instance.GenerationSpeed);
    }

    public void ClearAllEntities()
    {
        if (entityDictionary == null) { return; }

        if (entityDictionary.Count == 0) { return; }

        foreach (GameObject entity in entityDictionary.Values) { Destroy(entity); }

        entityDictionary.Clear();
    }

    void InitializeEntityDictionary() => entityDictionary = new Dictionary<_Tile, GameObject>();

    void GetSpawnablePositions() => spawnablePositions = new List<_Tile>(DungeonManager.Instance.ListWalkableTiles);

    void SpawnPlayer()
    {
        playerStartTile = spawnablePositions[0];
        Vector3 playerStartPosition = new Vector3(playerStartTile.worldPosition.x, playerStartTile.worldPosition.y, 0);

        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);

        entityDictionary.Add(playerStartTile, player);

        spawnablePositions.RemoveAt(0);
    }
}
}