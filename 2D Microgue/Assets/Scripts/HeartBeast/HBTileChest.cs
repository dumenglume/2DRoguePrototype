using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace HB
{
public class HBTileChest : HBTile
{
    public static event Action PickedUpItem;

    public HBTileChest(Vector3Int _localPosition, Tilemap _tilemap)
    {
        localPosition = _localPosition;
        tilemap       = _tilemap;
    }

    void TriggerTileEvent()
    {
        tilemap.RefreshTile(Vector3Int.RoundToInt(localPosition));
        entityObject.SetActive(false);

        PickedUpItem?.Invoke();
    }
}
}