using System;
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

    protected PlayerFood playerFood;
    public PlayerFood PlayerFood => playerFood;

    protected PlayerPower playerPower;
    public PlayerPower PlayerPower => playerPower;

    PlayerInput playerInput;
    float inputX, inputY;

    protected override void Awake()
    {
        base.Awake();

        playerInput    = GetComponent<PlayerInput>();
        entityMovement = GetComponent<EntityMovement>();
        playerXP       = GetComponent<PlayerXP>();
        playerGold     = GetComponent<PlayerGold>();
        playerFood     = GetComponent<PlayerFood>();
        playerPower    = GetComponent<PlayerPower>();

        PlayerSpawned?.Invoke(this);
    }

    void OnEnable() 
    {
        _ExitTile.ExitTileTriggered += SaveEntitySats;
        Pickup.PickedUpItem         += playerPower.ChangePowerCurrent;
    }

    void OnDisable()
    {
        _ExitTile.ExitTileTriggered -= SaveEntitySats;
        Pickup.PickedUpItem         -= playerPower.ChangePowerCurrent;
    }

    protected override void SetEntityStats()
    {
        base.SetEntityStats();

        playerPower.BindEntity(this);

        GameManager gameManager = GameManager.Instance;

        Debug.Log($"Game Manager: { gameManager }");

        if (gameManager.gameHasStarted)
        {
            playerPower.SetPowerMax(gameManager.playerPowerMax);
            playerPower.SetPowerCurrent(gameManager.playerPowerCurrent);
            playerXP.SetXPAmount(gameManager.playerXPCurrent);
            playerXP.SetNextXP(gameManager.playerXPNext);
            playerGold.SetGoldAmount(gameManager.playerGold);
            playerFood.SetFoodMaxAmount(gameManager.playerFoodMax);
            playerFood.SetFoodAmount(gameManager.playerFoodCurrent);
        }

        else
        {
            playerPower.SetPowerMax(entityStatsReference.PowerMax);
            playerPower.SetPowerCurrent(entityStatsReference.PowerCurrent);
            playerXP.SetXPAmount(0);
            playerXP.SetNextXP(25);
            playerGold.SetGoldAmount(0);
            playerFood.SetFoodMaxAmount(80);
            playerFood.SetFoodAmount(80);
        }

        entityMovement.SetMovementDuration(entityStatsReference.MovementDuration);
    }

    protected void SaveEntitySats() // TODO Change this to saving to a different location so it doesn't overwrite scriptable object
    {
        GameManager gameManager = GameManager.Instance;

        gameManager.SavePlayerPowerCurrent(playerPower.PowerCurrent);
        gameManager.SavePlayerPowerMax(playerPower.PowerMax);
        gameManager.SavePlayerXPCurrent(playerXP.CurrentXP);
        gameManager.SavePlayerXPNext(playerXP.CurrentXP);
        gameManager.SavePlayerGold(playerGold.CurrentGold);
        gameManager.SavePlayerFoodCurrent(playerFood.CurrentFood);
        gameManager.SavePlayerFoodMax(playerFood.FoodMax);
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

        if (inputIsDetected && !entityMovement.IsMoving && !entityCombat.IsAnimatingCombat)
        {
            entityMovement.AttemptToMove(x, y, Vector3Int.RoundToInt(inputDirection), SetEntityPosition);
        }
    }

    public override void TakeDamage(int damageAmount) => playerPower.ChangePowerCurrent(damageAmount);
}
}