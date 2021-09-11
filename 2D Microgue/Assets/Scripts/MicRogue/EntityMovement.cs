using System;
using UnityEngine;

namespace MicRogue
{
public class EntityMovement : MonoBehaviour
{
    public static event Action OnMovementComplete;

    public bool isMoving;
    public float timeBetweenMoves = 0.35f;

    public void Move(Vector2 _direction) // Called from base script to initiate movement
    {
        if (!isMoving)
        { 
            isMoving = true;
            LeanTween.move(gameObject, transform.position + (Vector3) _direction, timeBetweenMoves).setEaseInOutCubic().setOnComplete(MoveComplete);
        }
    }

    public void MoveComplete()
    {
        isMoving = false;

        // Triggers event notifying other scripts (mainly Game Manager) that movement is complete
        OnMovementComplete?.Invoke();
    }
}
}