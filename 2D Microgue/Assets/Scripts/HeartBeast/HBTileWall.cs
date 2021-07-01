using UnityEngine;
using UnityEngine.Tilemaps;

namespace HB
{
public class HBTileWall : HBTile
{
    public HBTileWall(Vector3Int _localPosition, Tilemap _tilemap )
    {
        localPosition = _localPosition;
        tilemap       = _tilemap;
    }
}
}