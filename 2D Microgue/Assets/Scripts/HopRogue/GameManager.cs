using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using HopRogue.Entities;

namespace HopRogue
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static event Action OnPopulateUI;
    public static event Action ResetButtons; // TODO Find better way of resetting buttons back to initial status -- use a UI Manager?

    public EntityState.PlayerState PlayerState { get; private set; }

    World _world;
    public Dungeon CurrentDungeon { get; private set; }

    [SerializeField] AudioController AudioController;

    [SerializeField] bool tweeningActive;

    void Awake() 
    {
        if   (Instance != null && Instance != this) { Destroy(this.gameObject); }
        else { Instance = this; }
    }

    void OnEnable() // TODO Create a master event handler script
    {
        ClickableSkill.OnSkillClicked       += ChangePlayerState;
        HopTileExit.ExitTileTriggered       += ProceedToNextFloor;
        InputController.MouseButtonClicked  += OnMouseButtonClicked;
        TurnManager.PlayerTurn              += HandlePlayerTurn;
        TurnManager.EnemyTurn               += HandleEnemyTurn;
    }

    void OnDisable()
    {
        ClickableSkill.OnSkillClicked       -= ChangePlayerState;
        HopTileExit.ExitTileTriggered       -= ProceedToNextFloor;
        InputController.MouseButtonClicked  -= OnMouseButtonClicked;
        TurnManager.PlayerTurn              -= HandlePlayerTurn;
        TurnManager.EnemyTurn               -= HandleEnemyTurn;
    }

    void Start()
    {
        GenerateNewGame();
    }

    void GenerateNewGame()
    {
        if (CurrentDungeon != null)
            CurrentDungeon.RemoveAllEntitiesFromDungeon();

        CreateNewWorldAndDungeon();
        AssignTargetEntities();
        PopulateUI();

        AudioController.PlaySFXStairs(); // TODO Find better location for this
        TurnManager.SetCurrentTurn(TurnManager.Turn.player);
    }

    void CreateNewWorldAndDungeon()
    {
        _world = World.Instance;
        _world.CreateWorld();
        CurrentDungeon = _world.CurrentDungeon;
    }

    void ProceedToNextFloor()
    {
        CurrentDungeon.RemoveAllEntitiesFromDungeon();
        _world.DestroyAllEntityGameObjects();
        GenerateNewGame();
    }

    void Update()
    {
        if (IsPlayerTurn())
            ListenForPlayerInput();

        tweeningActive = LeanTween.isTweening();
    }

    void OnMouseButtonClicked(int mouseButtonIndex)
    {
        switch(mouseButtonIndex)
        {
            case 0:
                ClickedOnTile(GetClickedTilePosition());
                break;

            case 1:
                ProceedToNextTurn();
                break;

            case 2:
                ProceedToNextFloor();
                break;

            default:
                break;
        }
    }

    void AssignTargetEntities()
    {
        Player player          = CurrentDungeon.Player;
        List<Monster> monsters = CurrentDungeon.Monsters;

        foreach (Monster monster in monsters)
            monster.AssignNewTarget(player);
    }

    void PopulateUI() => OnPopulateUI?.Invoke();

    void ChangePlayerState(bool stateEnabled, int stateIndex) // TODO Find better way to implement this
    {
        if (stateEnabled)
            PlayerState = (EntityState.PlayerState) stateIndex;

        else
            PlayerState = EntityState.PlayerState.normal;
    }

    Vector3Int GetClickedTilePosition()
    {
        Tilemap tilemap               = _world.Tilemap;
        Vector3 screenToWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tileWorldPosition  = tilemap.WorldToCell(screenToWorldPosition);

        Debug.Log($"Clicked on {(Vector2Int) tileWorldPosition}");
        
        return tileWorldPosition;
    }

    void ClickedOnTile(Vector3Int clickedPosition)
    {
        if (!CurrentDungeon.TileIsWithinBounds(clickedPosition))
            return;

        if (AnimationInProgress()) // ! Need to convert this into a separate variable so squash and strech animations can occur
            return;

        Player player = CurrentDungeon.Player;
        Entity selectedEntity = CurrentDungeon.GetEntityAtPosition(clickedPosition);

        if (selectedEntity != null)
            player.AssignNewTarget(selectedEntity);

        if (PlayerState == EntityState.PlayerState.shoot)
        {
            if (selectedEntity == null)
                return;

            if (CombatManager.IsTargetInLOS(player, selectedEntity))
            {
                selectedEntity.TakeDamage(player.AttackDamage);

                PlayerState = EntityState.PlayerState.normal;
                ResetButtons?.Invoke();
                ProceedToNextTurn();
                return;
            }

            else
                Debug.Log($"{selectedEntity.name} is out of range");
                return;
        }

        else if (PlayerState == EntityState.PlayerState.haste)
        {

        }

        else if (PlayerState == EntityState.PlayerState.wall)
        {
            player.PlayerSkill.SpawnEntityAtPostion(clickedPosition);
            PlayerState = EntityState.PlayerState.normal;
            ResetButtons?.Invoke();
            ProceedToNextTurn();
            return;
        }

        else
            StartCoroutine(Co_HandlePlayerTurn(player, clickedPosition));
    }

    IEnumerator Co_HandlePlayerTurn(Actor actor, Vector3Int targetPosition)
    {
        yield return actor.Co_HandleActorTurn(targetPosition, CurrentDungeon);

        ProceedToNextTurn();
    }

    IEnumerator Co_HandleEnemyTurn()
    {
        List<Monster> _monsters = CurrentDungeon.Monsters;

        foreach (Monster monster in _monsters)
        {
            monster.ResetActionsRemainingCount(); // TODO Move to a method inside actor script

            Vector3Int targetPosition = monster.TargetEntity ? monster.TargetEntity.WorldPosition : Vector3Int.zero;

            while (monster.HasActionsRemaining())
            {
                yield return monster.Co_HandleActorTurn(targetPosition, CurrentDungeon);
                monster.UseAction();
            }

            yield return null;
        }
        
        ProceedToNextTurn();
    }

    void RemoveEntity(Entity entity) => CurrentDungeon.RemoveEntity(entity);

    # region Pointer methods

    // Turn Manager pointers
    bool IsPlayerTurn()      => TurnManager.IsPlayerTurn();
    bool IsEnemyTurn()       => TurnManager.IsEnemyTurn();
    void HandlePlayerTurn()  => Debug.Log($"Player's Turn"); // TODO May not be needed
    void HandleEnemyTurn()   => StartCoroutine(Co_HandleEnemyTurn());
    void ProceedToNextTurn() => TurnManager.ProceedToNextTurn();

    // Animation Controller pointers
    bool AnimationInProgress() => AnimationController.AnimationInProgress();
    IEnumerator AnimateMovement(Entity entity, Vector3Int postionChange) => AnimationController.AnimateMovement(entity, postionChange);
    IEnumerator AnimateCombat(Actor attacker, Entity defender, Vector3Int startingPosition, Vector3Int attackDirection, Action<Actor, Entity> OnContact) =>
        AnimationController.AnimateCombat(attacker, defender, startingPosition, attackDirection, OnContact);

    // Input Controller pointers
    void ListenForPlayerInput() => InputController.ListenForPlayerInput();

    # endregion
}
}