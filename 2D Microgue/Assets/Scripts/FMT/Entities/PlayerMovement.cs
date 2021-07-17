using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class PlayerMovement : EntityMovement
{
    public static event Action<_Tile> OnMoveToTile;
    public static event Action<Player, Enemy, Vector3> OnTileInteraction;
    public static event Action OnMovementComplete;

    [SerializeField] Player player;

    [SerializeField] _Tile[,] tileGrid;

    void Awake() => player = player ?? GetComponent<Player>();

    void Start() { tileGrid = DungeonManager.Instance.tileGrid; }

    public override void AttemptToMove(int curX, int curY, Vector3 movementDirection, Action<int, int> CallbackNewPosition)
    {
        int destinationX = curX + (int) movementDirection.x;
        int destinationY = curY + (int) movementDirection.y;
        _Tile destinationTile = tileGrid[destinationX, destinationY];

        if (LeanTween.isTweening()) { return; }

        if (TileIsOccupied(destinationTile))
        {
            Enemy thisEnemy = destinationTile.boundGameObject.GetComponent<Enemy>();

            if (thisEnemy != null) OnTileInteraction?.Invoke(player, thisEnemy, movementDirection);
            CallbackNewPosition(curX, curY);

            return;
        }

        if (destinationTile is IAmWalkable) // TODO Check to see if this should be a static reference instead of a singleton reference
        {
            AnimateMovement(destinationTile);
            CallbackNewPosition(destinationX, destinationY);
            return;
        }
    }

    bool TileIsOccupied(_Tile tile) 
    { 
        return tile.IsOccupied;
    }

    void AnimateMovement(_Tile destinationTile)
    {
        Vector3Int tweenDestination = Vector3Int.RoundToInt(destinationTile.worldPosition);
        isMoving = true;
        BroadcastMovementStart(destinationTile);

        LeanTween.move(gameObject, tweenDestination, movementDuration).setEaseInOutQuad().setOnComplete(() => 
        {
            TriggerTile(destinationTile); // TODO Move to separate class?
            SetMovingToFalse();
            BroadcastMovementComplete();
        });
    }

    void TriggerTile(_Tile triggeredTile)
    {
        if (triggeredTile is ITrigerrable)
        {
            ITrigerrable thisTriggeredTile = triggeredTile as ITrigerrable;
            thisTriggeredTile.Trigger();
        }
    }

    void BroadcastMovementStart(_Tile _location) => OnMoveToTile?.Invoke(_location); // * Used for telling fog to clear at destination tile

    void BroadcastMovementComplete() => OnMovementComplete?.Invoke();
}
}