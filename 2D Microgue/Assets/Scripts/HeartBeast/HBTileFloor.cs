using UnityEngine;
using UnityEngine.Tilemaps;

namespace HB
{
public class HBTileFloor : HBTile
{
    public HBTileFloor(Vector3Int _localPosition, Tilemap _tilemap)
    {
        localPosition = _localPosition;
        tilemap       = _tilemap;
    }


}
}