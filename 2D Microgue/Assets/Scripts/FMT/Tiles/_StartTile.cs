using UnityEngine.Tilemaps;

namespace FMT
{
public class _StartTile : _Tile, IAmWalkable
{
    public _StartTile(TileBase tileSprite) : base(tileSprite) {}
}
}