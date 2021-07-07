using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class _ItemTile : _Tile, IAmWalkable, ITrigerrable
{
    public static event Action<Vector3Int> ItemTileTriggered;
    public _ItemTile(TileBase tileSprite) : base(tileSprite) {}
    public void Trigger() => ItemTileTriggered?.Invoke(Vector3Int.RoundToInt(worldPosition));
}
}