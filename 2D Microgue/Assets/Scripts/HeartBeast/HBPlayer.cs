using System;
using UnityEngine;

namespace HB {
public class HBPlayer : HBEntity
{
    public static event Action<HBPlayer> PlayerSpawned;

    protected HBEntityMovement entityMovement;
    public HBEntityMovement EntityMovement => entityMovement;

    protected HBPlayerXP playerXP;
    public HBPlayerXP PlayerXP => playerXP;

    protected HBPlayerGold playerGold;
    public HBPlayerGold PlayerGold => playerGold;

    HBPlayerInput playerInput;
    float inputX, inputY;

    protected override void Awake()
    {
        base.Awake();

        playerInput    = GetComponent<HBPlayerInput>();
        entityMovement = GetComponent<HBEntityMovement>();
        playerXP       = GetComponent<HBPlayerXP>();
        playerGold     = GetComponent<HBPlayerGold>();

        PlayerSpawned?.Invoke(this);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SetStats()
    {
        base.SetStats();

        entityMovement.SetMovementDuration(entityStats.MovementDuration);
    }

    void Update()
    {
        if (HBGameManager.gameState == HBGameManager.HBGameState.game)
        {
            InputPlayerMovement();
        }
    }

    void InputPlayerMovement()
    {
        inputX = playerInput.InputX;
        inputY = playerInput.InputY;

        Vector2 inputDirection = new Vector2(inputX, inputY);

        bool inputIsDetected = inputDirection.x != 0 || inputDirection.y != 0;

        if (inputIsDetected && !entityMovement.IsMoving && !entityCombat.IsInCombat)
        {
            entityMovement.AttemptToMove(inputDirection);
        }
    }
}
}