using System;
using UnityEngine;

namespace FMT
{
public class EntityMovement : MonoBehaviour
{
    [SerializeField] protected FloatReference movementDuration;
    public float MovementDuration => movementDuration.Value;
    
    protected bool isMoving = false;
    public bool IsMoving => isMoving;

    public void SetMovementDuration(float _timeInSeconds) { movementDuration.Value = _timeInSeconds; }

    public virtual void AttemptToMove(int curX, int curY, Vector3 movementDirection, Action<int, int> CallbackMovedToPosition) {}

    protected void SetMovingToFalse() { isMoving = false; }
}
}