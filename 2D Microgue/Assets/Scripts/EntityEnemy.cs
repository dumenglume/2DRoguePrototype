using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityEnemy : Entity
{
    public static event Action OnSkippedTurn;

    void OnEnable() 
    {
        GameManager.NotifyEnemyTurn += EnemyInput;
    }

    void OnDisable() 
    {
        GameManager.NotifyEnemyTurn -= EnemyInput;
    }

    void EnemyInput()
    {
        Debug.Log(name + "'s turn");

        bool isEnemy = (entityType == EntityType.enemy);

        if (isEnemy && hasTurn)
        { 
            Debug.Log(name + " is attempting to move");

            Vector2 inputDirection = entityInputController.DirectionOfTarget(); // Single call for enemies for performance

            Debug.Log("name is checking tile at " + inputDirection);

            if (HelperTilemaps.TileIsVacant(groundTilemap, collisionTilemap, transform.position, inputDirection, out TileBase _tile))
            {
                Debug.Log("Tile " + _tile + " is vacant");
                ReduceTurnCount(turnReductionAmount);
                entityMovement.Move(inputDirection);
            }

            else
            {
                Debug.Log(name + " could not move");
                OnSkippedTurn?.Invoke();
            }
        }
    }
}
