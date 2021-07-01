using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action NotifyPlayerTurn; // TODO Do something with this
    public static event Action NotifyEnemyTurn; // TODO Pass an argument to help specify the enemy?

    // Singleton
    static GameManager instance;
    public static GameManager Instance => instance;

    // Turn Management
    enum Turn { start, player, trap, enemies, won, lost }
    Turn currentTurn;
    int enemyTurnOrderIndex = 0;

    // Entity Tracking
    [SerializeField] Entity playerEntity;
    [SerializeField] List<Entity> enemiesList = new List<Entity>();

    [HideInInspector] public List<Entity> allEntitiesList = new List<Entity>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }

        else
        {
            instance = this;
        }
    }

    void Start()
    {
        InitializeEntitiesList();
        AssignFirstTurn();
    }

  void OnEnable()
    {
        EntityMovement.OnMovementComplete += ProceedToNextTurn;
        EntityEnemy.OnSkippedTurn         += ProceedToNextTurn;
    }

    void OnDisable()
    {
        EntityMovement.OnMovementComplete -= ProceedToNextTurn;
        EntityEnemy.OnSkippedTurn         -= ProceedToNextTurn;
    }

    void InitializeEntitiesList()
    {
        // Add player
        allEntitiesList.Add(playerEntity);

        // Add enemies
        if (enemiesList.Count > 0)
        {
            for (int i = 0; i < enemiesList.Count; i++)
            {
                allEntitiesList.Add(enemiesList[i]);
            }
        }
    }

    void AssignFirstTurn()
    {
        currentTurn = Turn.player;
        playerEntity.HasTurn = true;
    }

    void ProceedToNextTurn()
    {
        if (currentTurn == Turn.player) 
        {
            bool playerHasRemainingTurns = playerEntity.TurnsLeft > 0;

            if (playerHasRemainingTurns)
            { 
                PlayerTurn(playerEntity.TurnsLeft);
                return;
            }

            playerEntity.HasTurn = false;

            bool enemiesExist = enemiesList.Count > 0;

            if (enemiesExist)
            {
                ResetEnemyTurnOrder();
                Entity firstEnemy = enemiesList[enemyTurnOrderIndex];
                EnemyTurn(firstEnemy, firstEnemy.TurnCount);
            }
        }

        else if (currentTurn == Turn.enemies) 
        {
            Entity currentEnemy = enemiesList[enemyTurnOrderIndex];

            bool enemyHasRemainingTurns = currentEnemy.TurnsLeft > 0;

            if (enemyHasRemainingTurns)
            {
                EnemyTurn(currentEnemy, currentEnemy.TurnsLeft);
                return;
            }

            enemiesList[enemyTurnOrderIndex].HasTurn = false;

            IncreaseEnemyTurnOrder();

            if (enemyTurnOrderIndex < enemiesList.Count)
            {
                Entity nextEnemy = enemiesList[enemyTurnOrderIndex];
                EnemyTurn(nextEnemy, nextEnemy.TurnCount);
                return;
            }

            PlayerTurn(playerEntity.TurnCount);
        }
    }

    void PlayerTurn(int _turnsLeft)
    {
        currentTurn = Turn.player;

        playerEntity.TurnsLeft = _turnsLeft;
        playerEntity.HasTurn = true;

        NotifyPlayerTurn?.Invoke();
    }

    void EnemyTurn(Entity _enemy, int _turnsLeft)
    {
        currentTurn = Turn.enemies;

        _enemy.TurnsLeft = _turnsLeft; // TODO Consolidate this and below into one method
        _enemy.HasTurn = true;

        NotifyEnemyTurn?.Invoke();
    }

    void ResetEnemyTurnOrder()
    {
        enemyTurnOrderIndex = 0;
    }

    void IncreaseEnemyTurnOrder()
    {
        enemyTurnOrderIndex++;
    }
}
