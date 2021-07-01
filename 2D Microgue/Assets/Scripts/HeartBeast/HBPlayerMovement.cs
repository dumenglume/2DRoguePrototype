using System;
using UnityEngine;

namespace HB {
public class HBPlayerMovement : HBEntityMovement
{
    public static event Action<Vector3Int> OnMovementInitiated;
    public static event Action OnMovementComplete;

    [SerializeField] HBPlayer player;

    void Awake()
    {
        player = player ?? GetComponent<HBPlayer>();
    }

    public override void AttemptToMove(Vector3 _direction)
    {
        if (dungeonTileDictionary == null) { throw new System.Exception("No tile dictionary exists."); }

        Vector3 currentPosition     = transform.position;
        Vector3 destinationPosition = currentPosition + _direction;

        // if (dungeonTileDictionary.TryGetValue(destinationPosition, out HBTile destinationTile)) // ! Delete if no longer needed
        if (dungeonTileDictionary.ContainsKey(destinationPosition))
        {
            /*
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
            */
        }
    }

    void MoveToTile(HBTile _destinationTile)
    {
        if (LeanTween.isTweening()) { return; }

        Vector3Int destination = Vector3Int.FloorToInt(_destinationTile.localPosition);
        BroadcastMovement(destination);
        isMoving = true;

        LeanTween.move(gameObject, destination, movementDuration).setEaseInOutQuad().setOnComplete(() => 
        { 
            TriggerTile(_destinationTile); // ! Change this to something else
            SetMovingToFalse();
        });
    }

    void TriggerTile(HBTile _tile)
    {
        _tile.TriggerTileEvent();
    }

    void BroadcastMovement(Vector3Int _location)
    {
        OnMovementInitiated?.Invoke(_location);
    }
}
}