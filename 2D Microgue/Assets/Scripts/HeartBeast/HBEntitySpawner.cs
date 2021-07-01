using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HB {
    public class HBEntitySpawner : MonoBehaviour
    {
        public static event Action allEntitiesSpawned;
        public static event Action<Vector3Int> revealPlayerTile;

        static HBEntitySpawner instance;
        public static HBEntitySpawner Instance => instance;

        [SerializeField] HBDungeon dungeon;

        [SerializeField] int maxEnemies = 10;

        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject enemyPrefab;

        [SerializeField] List<GameObject> enemyPrefabList;

        static Dictionary<Vector3Int, GameObject> entityDictionary = new Dictionary<Vector3Int, GameObject>();
        public static Dictionary<Vector3Int, GameObject> EntityDictionary => entityDictionary;

        List<Vector3Int> spawnablePositions;
        public List<Vector3Int> SpawnablePositions => spawnablePositions;

        Vector3Int playerStartPosition;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
            }

            else
            {
                instance = this;
            }
        }

        void Start()
        {
            dungeon = HBDungeon.Instance;
        }

        void OnEnable()
        {
            HBDungeon.dungeonComplete      += SpawnEntities;
            HBDungeon.newDungeon           += ClearEntities;
            HBEntityHealth.PositionOfDeath  += RemoveEnemy;
        }

        void OnDisable()
        {
            HBDungeon.dungeonComplete      -= SpawnEntities;
            HBDungeon.newDungeon           -= ClearEntities;
            HBEntityHealth.PositionOfDeath  -= RemoveEnemy;
        }

        void ClearEntities()
        {
            if (entityDictionary.Count == 0) { return; }

            foreach (GameObject entity in entityDictionary.Values)
            {
                Destroy(entity);
            }

            entityDictionary.Clear();
        }

        void SpawnEntities()
        {
            spawnablePositions = new List<Vector3Int>(dungeon.WalkablePositions);

            SpawnPlayer();
            SpawnEnemies();
            BroadcastAllEntitiesSpawned();
            BroadcastRevealPlayerTile(playerStartPosition);
        }

        void SpawnPlayer()
        {
            playerStartPosition = spawnablePositions[0];

            GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity); // TODO May need to move to own function
            entityDictionary.Add(playerStartPosition, player);

            spawnablePositions.RemoveAt(0);
        }

        void SpawnEnemies()
        {
            // Dictionary<Vector3, HBTile> dungeonTileDictionary = dungeon.TileDictionary; // ! Delete this after testing
            Hashtable dungeonTileDictionary = dungeon.TileDictionary;

            int enemiesSpawned = 0;

            while(enemiesSpawned < maxEnemies)
            {
                int uniform = Mathf.FloorToInt(spawnablePositions.Count / maxEnemies);

                for (int i = 0; i < spawnablePositions.Count; i++)
                {
                    if (i % uniform == 0) // TODO Fix this later so that 10 enemies will not spawn twice as many enemies
                    {
                        Vector3Int enemyPosition = spawnablePositions[i];
                        int randomEnemyIndex     = Random.Range(0, enemyPrefabList.Count);
                        GameObject thisEnemy     = Instantiate(enemyPrefabList[randomEnemyIndex], enemyPosition, Quaternion.identity); // TODO May need to move to own function

                        // HBTile enemyTile       = dungeonTileDictionary[enemyPosition]; // ! Delete this after testing
                        HBTileEnemy enemyTile   = new HBTileEnemy(enemyPosition, dungeon.Tilemap);
                        print(enemyTile);
                        enemyTile.entityObject    = thisEnemy;

                        entityDictionary.Add(enemyPosition, thisEnemy);
                        spawnablePositions.RemoveAt(i);

                        enemiesSpawned ++;
                    }
                }
            }
        }

        public void RemoveEnemy(Vector3 _enemyPosition)
        {
            Vector3Int thisEnemyPosition = Vector3Int.RoundToInt(_enemyPosition);

            entityDictionary.Remove(thisEnemyPosition);
        }

        void BroadcastAllEntitiesSpawned() { allEntitiesSpawned?.Invoke(); }
        void BroadcastRevealPlayerTile(Vector3 _playerTilePosition) { revealPlayerTile?.Invoke(playerStartPosition); }
    }
}
