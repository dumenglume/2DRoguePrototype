using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HopRogue
{
[Serializable]
public class HopTileWall : HopTile
{
    public HopTileWall(int x, int y, Tilemap tilemap, TileBase tileBase, bool isWalkable = false, bool blocksLOS = true) : base(x, y, tilemap, tileBase, isWalkable, blocksLOS)
    {
    }
}
}