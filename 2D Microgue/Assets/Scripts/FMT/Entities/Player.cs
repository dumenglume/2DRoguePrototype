﻿using System;
using UnityEngine;

namespace FMT
{
public class Player : Entity
{
    public static event Action<Player> PlayerSpawned;

    protected EntityMovement entityMovement;
    public EntityMovement EntityMovement => entityMovement;

    protected PlayerXP playerXP;
    public PlayerXP PlayerXP => playerXP;

    protected PlayerGold playerGold;
    public PlayerGold PlayerGold => playerGold;

    PlayerInput playerInput;
    float inputX, inputY;

    protected override void Awake()
    {
        base.Awake();

        playerInput    = GetComponent<PlayerInput>();
        entityMovement = GetComponent<EntityMovement>();
        playerXP       = GetComponent<PlayerXP>();
        playerGold     = GetComponent<PlayerGold>();

        PlayerSpawned?.Invoke(this);
    }

    void OnEnable() => _ExitTile.ExitTileTriggered += SaveEntitySats;

    void OnDisable() => _ExitTile.ExitTileTriggered -= SaveEntitySats;

    protected override void SetEntityStats()
    {
        base.SetEntityStats();

        entityMovement.SetMovementDuration(entityStatsReference.MovementDuration);
    }

    protected void SaveEntitySats() // TODO Change this to saving to a different location so it doesn't overwrite scriptable object
    {
        entityStatsReference.HealthMax     = entityHealth.HealthMax;
        entityStatsReference.HealthCurrent = entityHealth.HealthCurrent;
        entityStatsReference.AttackPower   = entityCombat.AttackPower;
        entityStatsReference.PowerLevel    = entityCombat.PowerLevel;
    }

    void Update()
    {
        InputPlayerMovement();
    }

    void InputPlayerMovement()
    {
        inputX = playerInput.InputX;
        inputY = playerInput.InputY;

        Vector3 inputDirection = new Vector3(inputX, inputY, 0);

        bool inputIsDetected = inputDirection.x != 0 || inputDirection.y != 0;

        if (inputIsDetected && !entityMovement.IsMoving && !entityCombat.IsInCombat)
        {
            entityMovement.AttemptToMove(x, y, Vector3Int.RoundToInt(inputDirection), SetEntityPosition);
        }
    }
}
}