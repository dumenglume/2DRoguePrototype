using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HB
{
public class HBTileEnemy : HBTile
{
    public HBTileEnemy(Vector3Int _localPosition, Tilemap _tilemap)
    {
        localPosition = _localPosition;
        tilemap       = _tilemap;
    }

    void TriggerTileEvent()
    {
        
    }
}
}