using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class _FloorTile : _Tile, IAmWalkable, ITrigerrable
{
    public static Action OnVisitedTile;
    public static Action OnPickedUpItem;

    public _FloorTile(TileBase tileSprite) : base(tileSprite) {}

    public void Trigger() 
    {
        if (isVisited == false)
        {
            MarkAsVisited(true);
            OnVisitedTile?.Invoke();
        }

        if (gameObject == null) { return; }

        OnPickedUpItem?.Invoke();
        gameObject.SetActive(false);
    }
}
}