using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class PlayerMovement : EntityMovement
{
    public static event Action<_Tile> OnMoveToTile;
    public static event Action OnMovementComplete;

    [SerializeField] Player player;

    [SerializeField] _Tile[,] tileGrid;

    void Awake() => player = player ?? GetComponent<Player>();

    void Start() { tileGrid = DungeonManager.Instance.tileGrid; Debug.Log(tileGrid.Length); }

    /*
    public override void AttemptToMoveOld(Vector3 _direction)
    {
        if (dungeonTileDictionary == null) { throw new System.Exception("No tile dictionary exists."); }

        Vector3 currentPosition     = transform.position;
        Vector3 destinationPosition = currentPosition + _direction;

        // if (dungeonTileDictionary.TryGetValue(destinationPosition, out HBTile destinationTile)) // ! Delete if no longer needed
        if (dungeonTileDictionary.ContainsKey(destinationPosition))
        {
            var destinationTile = dungeonTileDictionary[destinationPosition];
            // if (destinationTile.IsWalkable) { MoveToTile(destinationTile); return; } // ! Delete if no longer needed
            if (destinationTile is HBTileFloor) { MoveToTile(destinationTile); return; }

            // else if (destinationTile.IsEnemy) // ! Delete if no longer needed
            else if (destinationTile is HBTileEnemy)
            {
                GameObject thisEnemyObject = HBEntitySpawner.EntityDictionary[Vector3Int.RoundToInt(destinationPosition)];
                if (thisEnemyObject == null) { throw new System.Exception("Enemy not found."); }

                HBEnemy thisEnemy = thisEnemyObject.GetComponent<HBEnemy>();
                HBCombatManager.CombatEncounter(player, thisEnemy, _direction);
            }
        }
    }
    */

    public override void AttemptToMove(int curX, int curY, int destX, int destY, Action<int, int> CallbackMovedToPosition)
    {
        int destinationX = curX + destX;
        int destinationY = curY + destY;
        _Tile destinationTile = tileGrid[destinationX, destinationY];

        if (destinationTile is IAmWalkable) // TODO Check to see if this should be a static reference instead of a singleton reference
        {
            MoveToTile(destinationTile);
            CallbackMovedToPosition(destinationX, destinationY); // TODO May not be necessary
            return;
        }

        if (destinationTile is IAmInteractive)
        {
            // Do combat
            return;
        }
    }

    void MoveToTile(_Tile destinationTile)
    {
        if (LeanTween.isTweening()) { return; }

        Vector3Int tweenDestination = Vector3Int.RoundToInt(destinationTile.worldPosition);
        BroadcastMovement(destinationTile);
        isMoving = true;

        LeanTween.move(gameObject, tweenDestination, movementDuration).setEaseInOutQuad().setOnComplete(() => 
        {
            TriggerTile(destinationTile); // TODO Move to separate class?
            SetMovingToFalse();
        });
    }

    void TriggerTile(_Tile triggeredTile)
    {
        if (triggeredTile is ITrigerrable)
        {
            ITrigerrable thisTriggeredTile = triggeredTile as ITrigerrable;
            thisTriggeredTile.Trigger();
            Debug.Log($"{ triggeredTile } trigged");
        }
    }

    void BroadcastMovement(_Tile _location)
    {
        OnMoveToTile?.Invoke(_location); // * Used for telling fog to clear at destination tile
    }
}
}