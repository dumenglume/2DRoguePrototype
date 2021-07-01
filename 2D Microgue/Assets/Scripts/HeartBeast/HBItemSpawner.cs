using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HB {
    public class HBItemSpawner : MonoBehaviour
    {
        public static event Action allItemsSpawned;

        static HBItemSpawner instance;
        public static HBItemSpawner Instance => instance;

        HBDungeon dungeon;
        HBEntitySpawner entitySpawner;

        [SerializeField] int maxItems = 10;
        [SerializeField] int maxChests = 3;

        [SerializeField] GameObject itemPrefab;
        [SerializeField] List<GameObject> itemPrefabs;
        [SerializeField] GameObject chestPrefab;

        List<GameObject> allChestAndItems = new List<GameObject>();

        List<Vector3Int> spawnablePositions;
        // Dictionary<Vector3, HBTile> dungeonTileDictionary; // ! Delete if no longer needed
        Hashtable dungeonTileDictionary;
        List<Vector3Int> deadEndPositionsList;


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
            dungeon       = HBDungeon.Instance;
            entitySpawner = HBEntitySpawner.Instance;
        }

        void OnEnable()
        {
            HBEntitySpawner.allEntitiesSpawned += SpawnChestsAndItems;
            HBDungeon.newDungeon               += ClearChestsAndItems;
        }

        void OnDisable()
        {
            HBEntitySpawner.allEntitiesSpawned -= SpawnChestsAndItems;
            HBDungeon.newDungeon               -= ClearChestsAndItems;
        }

        void ClearChestsAndItems()
        {
            if (allChestAndItems.Count == 0) { return; }

            for (int i = allChestAndItems.Count - 1; i >= 0 ; i--)
            {
                GameObject itemToRemove = allChestAndItems[i];
                Destroy(itemToRemove);
                allChestAndItems.RemoveAt(i);
            }
        }

        void SpawnChestsAndItems()
        {
            spawnablePositions    = new List<Vector3Int>(entitySpawner.SpawnablePositions);
            dungeonTileDictionary = dungeon.TileDictionary;
            deadEndPositionsList  = new List<Vector3Int>(dungeon.DeadEndPositions);

            SpawnChests();
            SpawnItems();
            BroadcastAllItemsSpawned();
        }

        void SpawnChests()
        {
            int chestsSpawned = 0;
            int deadEndCount  = deadEndPositionsList.Count;

            while(chestsSpawned < maxChests)
            {
                int randomIndex = Random.Range(0, deadEndPositionsList.Count - 1); // NOTE -2 so that it doesn't choose the final space
                Vector3 randomDeadEndPosition = deadEndPositionsList[randomIndex];

                GameObject thisChest = Instantiate(chestPrefab, randomDeadEndPosition, Quaternion.identity); // TODO May need to move to own function

                // HBTile chestTile        = dungeonTileDictionary[randomDeadEndPosition]; // ! Delete this if no longer needed
                HBTileChest chestTile        = dungeonTileDictionary[randomDeadEndPosition] as HBTileChest;

                allChestAndItems.Add(thisChest);
                deadEndPositionsList.RemoveAt(randomIndex);

                chestsSpawned ++;
            }
        }

        void SpawnItems()
        {
            int itemsSpawned = 0;

            while(itemsSpawned < maxItems)
            {
                int randomIndex = Random.Range(0, spawnablePositions.Count - 1); // NOTE -2 so that it doesn't choose the final space
                Vector3 randomPosition = spawnablePositions[randomIndex];
                int randomItemIndex = Random.Range(0, itemPrefabs.Count);
                GameObject thisItem = Instantiate(itemPrefabs[randomItemIndex], randomPosition, Quaternion.identity); // TODO May need to move to own function

                // HBTile itemTile        = dungeonTileDictionary[randomPosition]; // ! Delete this if no longer needed
                HBTileItem itemTile    = dungeonTileDictionary[randomPosition] as HBTileItem;
                itemTile.entityObject    = thisItem;

                allChestAndItems.Add(thisItem);
                spawnablePositions.RemoveAt(randomIndex);

                itemsSpawned ++;
            }
        }

        void BroadcastAllItemsSpawned() { allItemsSpawned?.Invoke(); }
    }
}
