using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class _ExitTile : _Tile, ITrigerrable
{
    public static event Action<Vector3Int> ExitTileTriggered;

    public void Trigger()
    {
        ExitTileTriggered?.Invoke(Vector3Int.RoundToInt(coordinate));
    }
}