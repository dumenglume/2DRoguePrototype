using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue.Entities
{
public class Player : Actor
{
    [SerializeField] PlayerSkill _playerSkill;
    public PlayerSkill PlayerSkill => _playerSkill;

    protected override void Start()
    {
        GetPlayerGameStats();
        CreateAStar();
    }
    
    void GetPlayerGameStats()
    {
        _health           = GameStats.PlayerHealth;
        _healthMax        = GameStats.PlayerHealthMax;
        _attackDamage     = GameStats.AttackDamage;
        _distancePerMove  = GameStats.DistancePerMove;
        _actionsMax       = GameStats.ActionsMax;
        _actionsRemaining = GameStats.ActionsRemaining;
    }

    void SetPlayerGameStats()
    {
        GameStats.PlayerHealth     = _health;
        GameStats.PlayerHealthMax  = _healthMax;
        GameStats.AttackDamage     = _attackDamage;
        GameStats.DistancePerMove  = _distancePerMove;
        GameStats.ActionsMax       = _actionsMax;
        GameStats.ActionsRemaining = _actionsRemaining;
    }

    public void IncreaseHealthAndMaxHealh(int amount)
    {
        _healthMax += amount;
        _health += amount;
    }

    public override void ResolveDeath()
    {
        SetPlayerGameStats();
        base.ResolveDeath();
    }

    [ContextMenu("Move Towards Target")]
    public override List<Vector3Int> GetPathToTarget(Vector3Int targetPosition, Entity targetEntity, Dungeon dungeon, bool debug = false)
    {
        return base.GetPathToTarget(targetPosition, targetEntity, dungeon, debug);
    }

    public override IEnumerator Co_HandleActorTurn(Vector3Int targetPosition, Dungeon dungeon)
    {
        yield return base.Co_HandleActorTurn(targetPosition, dungeon);

        dungeon.TriggerTile(this, targetPosition); // TODO Change to event?
    }

    protected override IEnumerator HandleEntityEncounter(Vector3Int entityEncounteredPosition, Entity entityEncountered)
    {
        if (entityEncountered != TargetEntity)
            yield return null;

        if (entityEncountered is ITakeDamage)
        {
            yield return AnimateCombat(this, entityEncountered, WorldPosition, entityEncounteredPosition, OnContactAction); // * Change ResolveCombat to other actions to test them
            SetRemainingActionsToZero();
        }

        if (entityEncountered is ICanInteract)
        {
            ICanInteract interactiveEntity = entityEncountered as ICanInteract;
            interactiveEntity.PerformInteraction(this);
        }

        yield return null;
    }
}
}