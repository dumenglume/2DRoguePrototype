using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
    public class DungeonGenerator : MonoBehaviour
    {

        #region Variables ===============================================================================================================
        [SerializeField] int worldWidth  = 18;
        [SerializeField] int worldHeight = 12;

        // Tile/Tilemap references
        [SerializeField] Tilemap tilemap;
        [SerializeField] _Tile[] tilePrefabs;
        [DisplayOnly] public string[] tileTypes;
        [DisplayOnly] public List<_Tile> tiles = new List<_Tile>();
        public _Tile[,] tileGrid;
        public static DungeonGenerator instance;
        public static DungeonGenerator Instance => instance;
        #endregion Variables ===============================================================================================================

        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else
                instance = this;

            CreateTileTypes();
        }

        private void CreateTileTypes()
        {
            tileTypes = new string[tilePrefabs.Length];
            for (int i = 0; i < tilePrefabs.Length; i++)
            {
                tileTypes[i] = tilePrefabs[i].tileType;
            }
        }

        void Start()
        {
            tileGrid = new _Tile[worldWidth, worldHeight];
            Transform tilesContainer = CreateContainer("Tiles");

            // Generate grid data
            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight; y++)
                {
                    Vector3Int thisPosition = new Vector3Int(x, y, 0);
                    _Tile thisTile = Instantiate(tilePrefabs[UnityEngine.Random.Range(0, tilePrefabs.Length)], thisPosition, Quaternion.identity, tilesContainer) as _Tile;
                    thisTile.SetProperties(thisPosition, tilemap);
                    tiles.Add(thisTile);
                    tileGrid[x, y] = thisTile;
                }
            }

            // Draw grid
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].Initialize();
            }
        }

        private Transform CreateContainer(string name)
        {
            Transform container = new GameObject(name).transform;
            container.parent = transform;
            return container;
        }

        public _Tile GetTile (Vector2Int coordinates) {
            return tileGrid[coordinates.x, coordinates.y];
        }
    }
}