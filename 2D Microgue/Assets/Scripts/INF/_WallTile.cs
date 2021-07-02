using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;


public class _WallTile : _Tile
{
    protected override void SetState(bool interactive, bool walkable)
    {
        base.SetState(false, false);
    }
}
