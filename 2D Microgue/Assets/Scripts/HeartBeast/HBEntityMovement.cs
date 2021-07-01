using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB {
public class HBEntityMovement : MonoBehaviour
{
    [SerializeField] protected float movementDuration = 0.0f;
    public float MovementDuration => movementDuration;
    
    protected bool isMoving = false;
    public bool IsMoving => isMoving;

    // protected Dictionary<Vector3, HBTile> dungeonTileDictionary; // ! Delete this if no longer needed
    protected Hashtable dungeonTileDictionary;

    protected void OnEnable() { HBDungeon.dungeonComplete += GetTileDictionary; }

    protected void OnDisable() { HBDungeon.dungeonComplete -= GetTileDictionary; }

    protected void Start() { GetTileDictionary(); }

    protected void GetTileDictionary() { dungeonTileDictionary = HBDungeon.Instance.TileDictionary; }

    // public void SetTileType(Vector3 _tilePosition) { dungeonTileDictionary[_tilePosition].tileType = HBTile.HBTileType.floor; } // ! Delete this if no longer needed

    public void SetMovementDuration(float _timeInSeconds) { movementDuration = _timeInSeconds; }

    public virtual void AttemptToMove(Vector3 _direction) {}

    protected void SetMovingToFalse() { isMoving = false; }
}
}