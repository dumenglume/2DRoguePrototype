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

    public void SetMovementDuration(float _timeInSeconds) { movementDuration = _timeInSeconds; }

    public virtual void AttemptToMove(int curX, int curY, Vector3 movementDirection, Action<int, int> CallbackMovedToPosition) {}

    protected void SetMovingToFalse() { isMoving = false; }
}
}