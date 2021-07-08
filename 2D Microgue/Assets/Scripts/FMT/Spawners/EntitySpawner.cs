using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EntitySpawner : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] List<GameObject> enemyPrefabList;

    [SerializeField] int maxEnemies = 10;

    List<_Tile> spawnablePositions;
    _Tile playerStartTile;

    Dictionary<_Tile, GameObject> entityDictionary;
    public Dictionary<_Tile, GameObject> EntityDictionary => entityDictionary;

    float generationSpeed;

    public IEnumerator Co_SpawnEntities()
    {
        generationSpeed = DungeonManager.Instance.GenerationSpeed;

        InitializeEntityDictionary();
        GetSpawnablePositions();
        yield return Co_SpawnPlayer();
        yield return Co_SpawnEnemies();

        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
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

    IEnumerator Co_SpawnPlayer()
    {
        playerStartTile = spawnablePositions[0];
        Vector3 playerStartPosition = new Vector3(playerStartTile.worldPosition.x, playerStartTile.worldPosition.y, 0);

        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);

        entityDictionary.Add(playerStartTile, player);

        spawnablePositions.RemoveAt(0);

        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
    }

    IEnumerator Co_SpawnEnemies()
    {
        int enemiesSpawned = 0;

        while(enemiesSpawned < maxEnemies)
        {
            int uniform = Mathf.FloorToInt(spawnablePositions.Count / maxEnemies);

            for (int i = 0; i < spawnablePositions.Count; i++)
            {
                if (i % uniform == 0) // TODO Fix this later so that 10 enemies will not spawn twice as many enemies
                {
                    _Tile enemyTile      = spawnablePositions[i];
                    int randomEnemyIndex = Random.Range(0, enemyPrefabList.Count);

                    GameObject thisEnemy = Instantiate(enemyPrefabList[randomEnemyIndex], enemyTile.worldPosition, Quaternion.identity); // TODO May need to move to own function
                    enemyTile.gameObject = thisEnemy;

                    entityDictionary.Add(enemyTile, thisEnemy);
                    spawnablePositions.RemoveAt(i);

                    enemiesSpawned ++;

                    if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
                }
            }
        }
    }
}
}