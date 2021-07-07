using UnityEngine.Tilemaps;

namespace FMT
{
public class _EnemyTile : _Tile, IAmInteractive
{
    public _EnemyTile(TileBase tileSprite) : base(tileSprite) {}
}
}