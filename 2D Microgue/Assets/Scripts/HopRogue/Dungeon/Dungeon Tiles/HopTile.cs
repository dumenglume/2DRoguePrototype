using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using HopRogue.Entities;

namespace HopRogue
{
[Serializable]
public abstract class HopTile
{
    public int X { get; set; }
    public int Y { get; set; }
    public Vector3Int Position { get; set; }
    public bool IsWalkable { get; set; }
    public bool HasEntity { get; set; }
    public bool BlocksLOS { get; set; }

    public bool IsSpawnable { get; protected set; }
    public bool HasTrigger { get; protected set; } // TODO Change this to interface

    public HopTile(int x, int y, Tilemap tilemap, TileBase tileBase, bool isWalkable = false, bool blocksLOS = false)
    {
        X = x;
        Y = y;
        Position   = new Vector3Int(x, y, 0);
        Tilemap    = tilemap;
        TileBase   = tileBase;
        IsWalkable = isWalkable;
        BlocksLOS  = blocksLOS;
        HasEntity  = false;

        DrawTile();
    }

    // [NonSerialized]
    public GameObject baseObject;
    public Tilemap Tilemap { get; protected set; }
    public TileBase TileBase { get; protected set; }

    public void DrawTile() => Tilemap.SetTile(Position, TileBase);

    public virtual void TriggerTile(Actor actorThatTriggeredTile) {}
}
}