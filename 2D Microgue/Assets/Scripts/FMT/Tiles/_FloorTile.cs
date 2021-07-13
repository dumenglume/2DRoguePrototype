using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace FMT
{
public class _FloorTile : _Tile, IAmWalkable, ITrigerrable
{
    public static Action OnVisitedTile;

    public _FloorTile(TileBase tileSprite) : base(tileSprite) {}

    public void Trigger() 
    {
        if (isVisited == false)
        {
            MarkAsVisited(true);
            OnVisitedTile?.Invoke();
        }

        if (boundGameObject == null) { return; }

        IAmPickupable pickupObject = boundGameObject.GetComponent<IAmPickupable>();

        if (pickupObject == null) { return; }

        pickupObject.TriggerPickup();
        BindGameObjectToTile(null);
    }
}
}