using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class _ItemTile : _Tile, ITrigerrable
{
    public static event Action<Vector3Int> ItemTileTriggered;
    public void Trigger() => ItemTileTriggered?.Invoke(Vector3Int.RoundToInt(position));
}