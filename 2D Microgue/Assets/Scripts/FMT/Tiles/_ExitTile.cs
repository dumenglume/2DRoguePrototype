using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class _ExitTile : _Tile, IAmWalkable, ITrigerrable
{
    public static Action ExitTileTriggered;
    public _ExitTile(TileBase tileSprite) : base(tileSprite) {}
    public void Trigger() 
    { 
        Debug.Log("Exiting level");
        ExitTileTriggered?.Invoke();
    }
}
}