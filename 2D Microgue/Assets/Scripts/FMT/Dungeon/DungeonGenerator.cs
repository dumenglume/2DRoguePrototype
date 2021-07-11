using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace FMT
{
public class DungeonGenerator : MonoBehaviour
{
    #region Variables ==================================================================================================================

    [Header("World Dimensions")]
    [SerializeField] int worldWidth  = 18;
    [SerializeField] int worldHeight = 12;
    public int WorldWidth => worldWidth;
    public int WorldHeight => worldHeight;

    _Tile[,] tileGrid;
    public _Tile[,] TileGrid => tileGrid;

    [Header("Tilemap Settings")]
    [SerializeField] Tilemap tilemap;

    [Header("Tilemap Settings")]
    [SerializeField] TileBase floorTileBase;
    [SerializeField] TileBase wallTileBase;
    [SerializeField] TileBase startTileBase;
    [SerializeField] TileBase exitTileBase;

    // Collections
    [SerializeField] Dictionary<Vector3Int, _Tile> dictionaryWalkableTiles;
    public Dictionary<Vector3Int, _Tile> DictionaryWalkableTiles => dictionaryWalkableTiles; // TODO Change this to a custom List or HashSet
    int walkablePositionsLimit = 80;
    public int WalkablePositionsLimit => walkablePositionsLimit;
    [SerializeField] List<_Tile> listDeadEndTiles;
    public List<_Tile> ListDeadEndTiles => listDeadEndTiles;

    // Generation Speed Settings
    float generationSpeed = 0.0f;

    #endregion Variables ================================================================================================================
    #region Generation ==================================================================================================================

    void Start()
    {
        generationSpeed = DungeonManager.Instance.GenerationSpeed;
    }

    public IEnumerator Co_BeginGenerationProcess()
    {
        InitializeCollections();
        yield return StartCoroutine(Co_GenerateDungeon());
    }

    public void ClearAllTiles() => tilemap.ClearAllTiles();

    void InitializeCollections()
    {
        if (dictionaryWalkableTiles != null) { dictionaryWalkableTiles.Clear(); }
        dictionaryWalkableTiles = new Dictionary<Vector3Int, _Tile>();

        if (listDeadEndTiles != null) { listDeadEndTiles.Clear(); }
        listDeadEndTiles = new List<_Tile>();
    }

    IEnumerator Co_GenerateDungeon()
    {
        Debug.Log("Setting up grid");
        InitializeMap();

        Debug.Log("Filling map");
        FloodFillMap();

        Debug.Log("Getting random tile");
        _Tile startTile = GetRandomTile();

        Debug.Log("Marking start tile");
        yield return Co_MarkStartTile(startTile);

        Debug.Log("Creating walker");
        yield return Co_CreateWalker(startTile);

        Debug.Log("Marking exit tile");
        yield return Co_MarkExitTile(startTile);

        Debug.Log("Checking for dead ends");
        yield return Co_MarkDeadEnds();

        Debug.Log("Dungeon generated");
        Debug.Log($"Tile Grid: { tileGrid.Length }, Walkable Tiles: { dictionaryWalkableTiles.Count }, DeadEnds: { listDeadEndTiles.Count }");
    }

    void InitializeMap() => tileGrid = new _Tile[worldWidth, worldHeight];

    void FloodFillMap()
    {
        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                _WallTile wallTile = new _WallTile(wallTileBase);
                PlaceTile(x, y, wallTile);
            }
        }
    }

    IEnumerator Co_MarkStartTile(_Tile startPosition)
    {
        _StartTile startTile = new _StartTile(startTileBase);
        PlaceTile(startPosition.worldPosition.x, startPosition.worldPosition.y, startTile);
        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
    }

    IEnumerator Co_CreateWalker(_Tile startTile)
    {
        DungeonWalker walker = new DungeonWalker(startTile.worldPosition.x, startTile.worldPosition.y);
        walker.dungeonGenerator = this;
        yield return StartCoroutine(walker.Co_CarvePathway(floorTileBase));
    }

    IEnumerator Co_MarkExitTile(_Tile startTile)
    {
        _ExitTile exitTile = new _ExitTile(exitTileBase);
        _Tile furthestTile = GetFurthestTileGrid(startTile);
        PlaceTile(furthestTile.worldPosition.x, furthestTile.worldPosition.y, exitTile);
        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
    }

    _Tile GetFurthestTileGrid(_Tile tileToCheck)
    {
        _Tile currenTile       = tileToCheck;
        _Tile furthestTile     = null;
        float furthestDistance = 0;

        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                _Tile tileBeingChecked = GetTile(x, y);

                if (tileBeingChecked is IAmWalkable)
                {
                    float tileDistance = Vector3Int.Distance(tileBeingChecked.worldPosition, currenTile.worldPosition);

                    if (tileDistance > furthestDistance)
                    {
                        furthestTile     = tileBeingChecked;
                        furthestDistance = tileDistance;
                    }
                }
            }
        }

        return furthestTile;
    }

    IEnumerator Co_MarkDeadEnds()
    {
        int deadEndCount = 0;

        foreach (_Tile tile in dictionaryWalkableTiles.Values)
        {
            tile.cardinalNeighbors = GetCardinalNeighborTiles(tile);

            if (GetWalkableNeighborCount(tile) == 1)
            {
                tile.IsDeadEnd = true;
                deadEndCount ++;
                listDeadEndTiles.Add(tile);
            }
        }

        Debug.Log($"Dead End Count: {deadEndCount}");

        if (generationSpeed > 0.0f) { yield return new WaitForSeconds(generationSpeed); }
    }

    #endregion Generation ===================================================================================================================
    #region Helper Methods ==================================================================================================================

    public void PlaceTile(int x, int y, _Tile newTile)
    {
        newTile.SetProperties(new Vector3Int(x, y, 0), tilemap);
        newTile.DrawTile();
        tileGrid[x, y] = newTile;

        if (newTile is IAmWalkable) { AddTileToWalkableList(newTile); }
    }
    
    public _Tile GetTile (int x, int y) 
    {
        return tileGrid[x, y];
    }

    _Tile GetRandomTile()
    {
        int randomX = (int) Random.Range(1, worldWidth - 1);
        int randomY = (int) Random.Range(1, worldHeight - 1);

        _Tile randomTile = GetTile(randomX, randomY);

        return randomTile;
    }

    _Tile[] GetCardinalNeighborTiles (_Tile centerTile)
    {
        _Tile[] tilesArray = centerTile.cardinalNeighbors;

        tilesArray[0] = GetTile(centerTile.worldPosition.x, centerTile.worldPosition.y + 1);
        tilesArray[1] = GetTile(centerTile.worldPosition.x + 1, centerTile.worldPosition.y);
        tilesArray[2] = GetTile(centerTile.worldPosition.x, centerTile.worldPosition.y - 1);
        tilesArray[3] = GetTile(centerTile.worldPosition.x - 1, centerTile.worldPosition.y);

        return tilesArray;
    }

    int GetWalkableNeighborCount(_Tile tile)
    {
        int neighborCount = 0;

        foreach (_Tile neighborTile in tile.cardinalNeighbors)
        {
            if (neighborTile is IAmWalkable)
            {
                neighborCount++;
            }
        }

        return neighborCount;
    }

    public void AddTileToWalkableList(_Tile newTile)
    {
        Vector3Int newTilePosition = newTile.worldPosition;
        dictionaryWalkableTiles[newTilePosition] = newTile;
    }

    #endregion Helper Methods ==================================================================================================================
}
}