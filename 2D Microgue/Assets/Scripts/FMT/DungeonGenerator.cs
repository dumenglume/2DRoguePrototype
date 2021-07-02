using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
    public class DungeonGenerator : MonoBehaviour
    {
        // Tile/Tilemap references
        [SerializeField] Tilemap tilemap;
        [SerializeField] _Tile[] tiles;
        [SerializeField] [DisplayOnly] private string[] tileTypes;
        [DisplayOnly] [SerializeField] private List<_Tile> freespaces;

        // Map will include 16 x 10
        [SerializeField] int worldWidth  = 18;
        [SerializeField] int worldHeight = 12;

        public static DungeonGenerator instance;
        public static DungeonGenerator Instance => instance;

        public _Tile[,] tilesCollection;

        [DisplayOnly] [SerializeField] private List<_Tile> _Tiles = new List<_Tile>();

        void Awake()
        {
            if (instance != null && instance != this)
                Destroy(this.gameObject);
            else
                instance = this;

            CreateTileTypes();
            freespaces = new List<_Tile>();
        }

        private void CreateTileTypes()
        {
            tileTypes = new string[tiles.Length];
            for (int i = 0; i < tiles.Length; i++)
            {
                tileTypes[i] = tiles[i].tileType;
            }
        }

        void Start()
        {
            tilesCollection = new _Tile[worldWidth, worldHeight];
            Transform tilesContainer = CreateContainer("Tiles");

            // Generate grid data
            for (int x = 0; x < worldWidth; x++)
            {
                for (int y = 0; y < worldHeight; y++)
                {
                    Vector3Int thisPosition = new Vector3Int(x, y, 0);
                    
                    _Tile thisTile = Instantiate(tiles[UnityEngine.Random.Range(0, tiles.Length)], thisPosition, Quaternion.identity, tilesContainer) as _Tile;
                    thisTile.SetProperties(thisPosition, tilemap);

                    if (thisTile.tileState.walkable)
                    {
                        Debug.Log("Tile Type:" + thisTile.tileType);
                        freespaces.Add(thisTile);
                    }

                    _Tiles.Add(thisTile);
                    tilesCollection[x, y] = thisTile;
                }
            }

            // Draw grid
            for (int i = 0; i < _Tiles.Count; i++)
            {
                _Tiles[i].Initialize();
            }

            Vector2Int r = new Vector2Int(UnityEngine.Random.Range(0, worldWidth), UnityEngine.Random.Range(0, worldHeight));

            _Tile t = GetTile(r);

            Debug.Log(t.tileState.visited);
            Debug.Log(t.tileState.walkable);
            Debug.Log(t.tileState.interactive);
            Debug.Log(t.coordinate);
        }

        private Transform CreateContainer(string name)
        {
            Transform container = new GameObject(name).transform;
            container.parent = transform;
            return container;
        }

        public _Tile GetTile (Vector2Int coordinates) {
            return tilesCollection[coordinates.x, coordinates.y];
        }
    }
}