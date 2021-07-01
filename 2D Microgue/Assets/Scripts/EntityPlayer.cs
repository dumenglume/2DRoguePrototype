using UnityEngine;
using UnityEngine.Tilemaps;

public class EntityPlayer : Entity
{
    Vector2 inputDirection;

    void OnEnable() 
    {
        GameManager.NotifyPlayerTurn += PlayerAnnouncesTurn;
    }

    void OnDisable() 
    {
        GameManager.NotifyPlayerTurn -= PlayerAnnouncesTurn;
    }

    void Update()
    {
        bool isPlayer = (entityType == EntityType.player);

        if (isPlayer && hasTurn)
        {
            HandlePlayerInput();
            HandlePlayerButtons();
        }
    }

    void PlayerAnnouncesTurn()
    {
        Debug.Log(name + "'s turn");
    }

    void HandlePlayerInput()
    {
        inputDirection = Vector2.zero;

        float inputX = entityInputController.InputX;
        float inputY = entityInputController.InputY;

        inputDirection = new Vector2(inputX, inputY);

        bool inputIsDetected = inputDirection.x != 0 || inputDirection.y != 0;
        bool isMoving = entityMovement.isMoving;

        if (inputIsDetected && !isMoving)
        {
            AttemptMovement(inputDirection);
        }
    }

    void AttemptMovement(Vector2 _inputDirection)
    {
        if (!HelperTilemaps.TileIsVacant(groundTilemap, collisionTilemap, transform.position, _inputDirection, out TileBase _tile))
        { 
            // Do things based on what the obstacle is
            Debug.Log(_tile + " is in the way.");
            return;
        }

        ReduceTurnCount(turnReductionAmount);
        entityMovement.Move(_inputDirection);
    }

    void HandlePlayerButtons()
    {
        if (entityInputController.IsFire1Pressed) 
        {
            // Do things
        }
    }
}
