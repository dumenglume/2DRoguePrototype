using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class _Tile
{
    #region Variables =============================================================================================================================================
    public Vector3Int worldPosition  {get; protected set;}
    public int x { get; protected set; }
    public int y { get; protected set; }
    public Tilemap    tilemap   {get; protected set;}
    public TileBase   tileBase;  // {get; protected set;}
    public _Tile[] cardinalNeighbors = new _Tile[4]; // North, East, South, West
    public GameObject gameObject;

    [SerializeField] bool isVisited;
    [SerializeField] bool isDeadEnd;
    public bool IsVisited { get => isVisited; set => isVisited = value; }
    public bool IsDeadEnd { get => isDeadEnd; set => isDeadEnd = value; }

    #endregion Variables ===========================================================================================================================================
    
    public _Tile(TileBase tileSprite)
    {
        tileBase = tileSprite;
    }

    public void SetProperties(Vector3Int position, Tilemap tilemap)
    {
        this.worldPosition = position;
        this.x = position.x;
        this.y = position.y;
        this.tilemap  = tilemap;
    }

    public void DrawTile() => tilemap.SetTile(worldPosition, tileBase);

    public void UpdateTile() => tilemap.SetTile(worldPosition, tileBase);
}
}