using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using HopRogue.Entities;

namespace HopRogue
{
[Serializable]
public class HopTileFloor : HopTile
{
    public HopTileFloor(int x, int y, Tilemap tilemap, TileBase tileBase, bool isWalkable = true, bool blocksLOS = false) : base(x, y, tilemap, tileBase, isWalkable, blocksLOS)
    {
        IsWalkable  = true;
        IsSpawnable = true;
    }

    public override void TriggerTile(Actor actorThatTriggeredTile) => Debug.Log("Stepped on floor tile.");
}
}