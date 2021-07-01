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
    [SerializeField] List<TileBase> tilePrefabs;

    // Map will include 16 x 10
    [SerializeField] int worldWidth  = 18;
    [SerializeField] int worldHeight = 12;

    public static DungeonGenerator instance;
    public static DungeonGenerator Instance => instance;

    DungeonTile[,] dungeonGrid;

    List<DungeonTile> dungeonTiles = new List<DungeonTile>();

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this.gameObject);
        else
            instance = this;
    }

    void Start()
    {
        dungeonGrid = new DungeonTile[worldWidth, worldHeight];

        // Generate grid data
        for (int x = 0; x < worldWidth; x++)
        {
            for (int y = 0; y < worldHeight; y++)
            {
                Vector3Int thisPosition = new Vector3Int(x, y, 0);

                DungeonTile thisTile = new DungeonTile(thisPosition, tilemap, tilePrefabs[0]);

                dungeonTiles.Add(thisTile);

                dungeonGrid[x, y] = thisTile;
            }
        }

        // Draw grid
        for (int i = 0; i < dungeonTiles.Count; i++)
        {
            DungeonTile thisTile = dungeonTiles[i];

            thisTile.Tilemap.SetTile(thisTile.Position, thisTile.TileBase);
        }
    }
}
}