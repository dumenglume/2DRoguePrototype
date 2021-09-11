using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HopRogue.Grid;
using Random = UnityEngine.Random;

namespace HopRogue.Entities
{
public abstract class Actor : Entity, IBlockMovement
{
    # region Stat Variables

    [SerializeField] protected int _attackDamage = 1;
    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }

    [SerializeField] protected Vector2Int _attackRange = Vector2Int.one;
    public Vector2Int AttackRange { get => _attackRange; set => _attackRange = value; }

    [SerializeField] protected int _distancePerMove = 1;
    public int MoveDistance { get => _distancePerMove; set => _distancePerMove = value; }

    [SerializeField] protected int _actionsRemaining = 1; // * Experimental
    public int ActionsRemaining { get => _actionsRemaining; set => _actionsRemaining = value; }

    [SerializeField] protected int _actionsMax = 1; // * Experimental
    public int ActionsMax { get => _actionsMax; set => _actionsMax = value; }

    # endregion
    # region Pathfinding Variables

    [SerializeField] AStar _aStar;
    [SerializeField] List<Vector3Int> _currentPath;
    public List<Vector3Int> CurrentPath { get => _currentPath; set => _currentPath = value; }
    public Entity TargetEntity { get; protected set; }
    [SerializeField] protected GameObject debugObject;

    [SerializeField] GameObject attackParticles;

    # endregion
    # region Base Methods

    protected override void Start()
    {
        base.Start();
        CreateAStar(); // ! Move this to GameManager?
    }

    # endregion
    # region Pathfinding Methods

    protected void CreateAStar() => _aStar = new AStar(GameManager.Instance.CurrentDungeon); // ! Move this to AssignNewTarget or refactor to where singleton isn't needed

    public void AssignNewTarget(Entity newTarget)
    {
        TargetEntity = newTarget ?? null;
        FlipSpriteTowardsTarget();
    }
    
    public virtual List<Vector3Int> GetPathToTarget(Vector3Int targetPosition, Entity targetEntity, Dungeon dungeon, bool debugPath = false)
    {
        List<Vector3Int> path = _aStar.CalculatePath(this.WorldPosition, targetPosition, targetEntity);

        return path;
    }

    # endregion
    # region Movement Methods

    public bool AttemptMovement(Vector3Int positionOffset, Dungeon dungeon, out Entity blockingEntity)
    {
        Vector3Int targetPosition = WorldPosition + positionOffset;
        bool tileIsValid          = dungeon.TileIsValid(targetPosition);
        blockingEntity            = dungeon.GetEntityAtPosition(targetPosition);

        if (!tileIsValid || blockingEntity != null)
            return false;
        
        MoveEntity(targetPosition);
        return true;
    }

    protected void MoveEntity(Vector3Int targetPosition)
    {
        BroadcastMove(WorldPosition, targetPosition);
        WorldPosition = targetPosition;

        // TODO Find better location for this later
        EntityAudio.PlaySFXWalk();
    }

    public void SetMoveDistance(int amount) => _distancePerMove = amount;

    public void ResetMoveDistance() => _distancePerMove = 1;

    public void ResetActionsRemainingCount() => _actionsRemaining = _actionsMax;

    public void UseAction() => _actionsRemaining--;

    public bool HasActionsRemaining() => _actionsRemaining > 0;

    protected void SetRemainingActionsToZero() => _actionsRemaining = 0;

    # endregion

    # region Interaction Methods

    public virtual IEnumerator Co_HandleActorTurn(Vector3Int targetPosition, Dungeon dungeon)
    {
        CurrentPath = GetPathToTarget(targetPosition, TargetEntity, dungeon, false);

        if (CurrentPath == null)
            yield break;

        Vector3Int nextPosition = CurrentPath[0] - WorldPosition;

        FlipSpriteTowardsTarget();

        if (AttemptMovement(nextPosition, dungeon, out Entity blockingEntity))
            yield return AnimateMovement(this, WorldPosition);

        else
            yield return HandleEntityEncounter(nextPosition, blockingEntity);

        yield return null;
    }

    bool MoveToRandomValidNeighborTile(Dungeon dungeon, out Vector3Int alternateMove)
    {
        List<Vector3Int> possibleMoves = dungeon.GetCardinalValidNeighbors(WorldPosition);
        alternateMove = Vector3Int.zero;

        if (possibleMoves.Count > 0)
        {
            Vector3Int randomAlternateMove = possibleMoves[Random.Range(0, possibleMoves.Count)];
            alternateMove                  = randomAlternateMove;
            return true;
        }

        else
        {
            Debug.Log($"{this.name} found no directions to move to");
            return false;
        }
    }

    protected virtual IEnumerator HandleEntityEncounter(Vector3Int entityEncounteredPosition, Entity entityEncountered)
    {
        Debug.Log($"{this.name} can't resolve entity interaction");
        yield return null;
    }

    protected void OnContactAction(Entity attacker, Entity defender)
    {
        attacker.EntityAudio.PlaySFXAttack(); // TODO Change this to event based audio vs. referencing directly
        SpawnAttackParticles(defender.transform.position);
        defender.TakeDamage();
    }

    # endregion
    # region Animation Methods

    protected bool AnimationInProgress() => AnimationController.AnimationInProgress();

    protected IEnumerator AnimateMovement(Entity entity, Vector3Int postionChange) => AnimationController.AnimateMovement(entity, postionChange);

    protected IEnumerator AnimateCombat(Actor attacker, Entity defender, Vector3Int startingPosition, Vector3Int attackDirection, Action<Actor, Entity> OnContact) =>
        AnimationController.AnimateCombat(attacker, defender, startingPosition, attackDirection, OnContact);

    protected IEnumerator AnimateProjectile(GameObject projectileObject, Entity defender, float distanceFromTarget, Action OnContact) =>
        AnimationController.AnimateProjectile(projectileObject, defender.WorldPosition, distanceFromTarget, OnContact);

    protected void FlipSpriteTowardsTarget()
    {
        if (TargetEntity == null) return;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); // TODO Move this to Awake method?
        
        if (WorldPosition.x > TargetEntity.WorldPosition.x)
            spriteRenderer.flipX = true;

        else if (WorldPosition.x < TargetEntity.WorldPosition.x)
            spriteRenderer.flipX = false;
    }

    # endregion

    void SpawnAttackParticles(Vector3 spawnPosition) => Instantiate(attackParticles, spawnPosition, Quaternion.identity);
}
}