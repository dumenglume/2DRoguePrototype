using UnityEngine.Tilemaps;

namespace FMT
{
public class _FloorTile : _Tile, IAmWalkable
{
    public _FloorTile(TileBase tileSprite) : base(tileSprite) {}
}
}