using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class DungeonTile
{
    protected Vector3Int _position;
    protected Tilemap    _tilemap;
    protected TileBase   _tileBase;
    protected GameObject _gameObject;

    public Vector3Int Position   => _position;
    public Tilemap    Tilemap    => _tilemap;
    public TileBase   TileBase   => _tileBase;
    public GameObject GameObject => _gameObject;

    public DungeonTile(Vector3Int position, Tilemap tilemap, TileBase tileBase)
    {
        _position   = position;
        _tilemap    = tilemap;
        _tileBase   = tileBase;
    }

    protected virtual void TriggerTile() {}
}

public class FloorTile : DungeonTile
{
    public FloorTile(Vector3Int position, Tilemap tilemap, TileBase tileBase) : base(position, tilemap, tileBase) {}

    protected override void TriggerTile() {}
}

public class WallTile : DungeonTile
{
    public WallTile(Vector3Int position, Tilemap tilemap, TileBase tileBase) : base(position, tilemap, tileBase) {}

    protected override void TriggerTile() {}
}

public class StartTile : DungeonTile
{
    public StartTile(Vector3Int position, Tilemap tilemap, TileBase tileBase) : base(position, tilemap, tileBase) {}

    protected override void TriggerTile() {}
}

public class ExitTile : DungeonTile
{
    public static event Action<Vector3Int> ExitTileTriggered;

    public ExitTile(Vector3Int position, Tilemap tilemap, TileBase tileBase) : base(position, tilemap, tileBase) {}

    protected override void TriggerTile() { ExitTileTriggered?.Invoke(Vector3Int.RoundToInt(_position)); }
}

public class ItemTile : DungeonTile
{
    public static event Action<Vector3Int> ItemTileTriggered;

    public ItemTile(Vector3Int position, Tilemap tilemap, TileBase tileBase) : base(position, tilemap, tileBase) {}

    protected override void TriggerTile() { ItemTileTriggered?.Invoke(Vector3Int.RoundToInt(_position)); }
}

public class EnemyTile : DungeonTile
{
    public EnemyTile(Vector3Int position, Tilemap tilemap, TileBase tileBase) : base(position, tilemap, tileBase) {}

    protected override void TriggerTile() {}
}
}