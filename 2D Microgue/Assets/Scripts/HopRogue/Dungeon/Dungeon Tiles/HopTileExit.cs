using System;
using UnityEngine.Tilemaps;
using HopRogue.Entities;

namespace HopRogue
{
[Serializable]
public class HopTileExit : HopTile
{
    public static event Action ExitTileTriggered;

    public HopTileExit(int x, int y, Tilemap tilemap, TileBase tileBase, bool isWalkable = false, bool blocksLOS = true) : base(x, y, tilemap, tileBase, isWalkable, blocksLOS)
    {
        IsWalkable  = true;
        IsSpawnable = false;
        HasTrigger  = true;
    }

    public override void TriggerTile(Actor actorThatTriggeredTile) => ExitTileTriggered?.Invoke();
}
}