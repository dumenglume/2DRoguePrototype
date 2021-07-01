using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HB
{
public class HBTileExit : HBTile
{
    public static event Action<Vector3Int> ExitTileTriggered; // ! Remove "new" after testing

    public HBTileExit(Vector3Int _localPosition, Tilemap _tilemap )
    {
        localPosition = _localPosition;
        tilemap       = _tilemap;
    }

    void TriggerTileEvent()
    {
        ExitTileTriggered?.Invoke(Vector3Int.RoundToInt(localPosition));
    }
}
}