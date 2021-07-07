using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EntityMovement : MonoBehaviour
{
    [SerializeField] protected float movementDuration = 0.0f;
    public float MovementDuration => movementDuration;
    
    protected bool isMoving = false;
    public bool IsMoving => isMoving;

    // protected Dictionary<Vector3, HBTile> dungeonTileDictionary; // ! Delete this if no longer needed

    // protected void Start() { GetTileDictionary(); }

    // protected void GetTileDictionary() { dungeonTileDictionary = HBDungeon.Instance.TileDictionary; }

    // public void SetTileType(Vector3 _tilePosition) { dungeonTileDictionary[_tilePosition].tileType = HBTile.HBTileType.floor; } // ! Delete this if no longer needed

    public void SetMovementDuration(float _timeInSeconds) { movementDuration = _timeInSeconds; }

    public virtual void AttemptToMove(int curX, int curY, int destX, int destY, Action<int, int> CallbackMovedToPosition) {}

    protected void SetMovingToFalse() { isMoving = false; }
}
}