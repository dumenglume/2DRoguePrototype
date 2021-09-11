using System;
using UnityEngine.Tilemaps;

namespace HopRogue
{
[Serializable]
public class HopTileEmpty : HopTile
{
    public HopTileEmpty(int x, int y, Tilemap tilemap, TileBase tileBase, bool isWalkable = false, bool blocksLOS = true) : base(x, y, tilemap, tileBase, isWalkable, blocksLOS)
    {
        IsWalkable  = false;
        IsSpawnable = false;
        HasTrigger  = false;
    }
}
}