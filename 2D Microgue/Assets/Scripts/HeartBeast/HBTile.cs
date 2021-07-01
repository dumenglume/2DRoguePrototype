using System;
using UnityEngine;
using UnityEngine.Tilemaps;

// https://youtu.be/cb6rz64jZUo?t=520

namespace HB {
public class HBTile
{
    public Vector3Int localPosition { get; set; }
    public Tilemap tilemap { get; set; }
    public TileBase tileBase { get; set; }
    public GameObject entityObject { get; set; }

    public void TriggerTileEvent()
    {
    }
}
}