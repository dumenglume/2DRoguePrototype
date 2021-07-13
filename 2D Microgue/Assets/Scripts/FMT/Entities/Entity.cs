using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public abstract class Entity : MonoBehaviour
{
    public static event Action EntityDied;
    public static event Action<Vector3> KilledAtPosition;

    protected EntityCombat entityCombat;
    public EntityCombat EntityCombat => entityCombat;

    protected EntityPower entityPower;
    public EntityPower EntityPower => entityPower;

    [Header("Entity Settings")]
    [SerializeField] string entityName;
    public string EntityName => entityName;
    [SerializeField] protected EntityStats entityStatsReference;

    protected int x = 0;
    protected int y = 0;

    protected virtual void Awake() 
    {
        entityCombat = GetComponent<EntityCombat>();
        entityPower  = GetComponent<EntityPower>();
    }

    protected virtual void Start()
    {
        SetEntityStats();
        SetEntityPosition((int) transform.position.x, (int) transform.position.y);
    }

    protected virtual void SetEntityStats()
    {
        entityName = entityStatsReference.Name;
        entityPower.BindEntity(this);
        entityPower.SetPowerMax(entityStatsReference.PowerMax);
        entityPower.SetPowerCurrent(entityStatsReference.PowerCurrent);
        entityCombat.SetBumpDuration(entityStatsReference.BumpDuration);
    }

    public void SetEntityPosition(int xPosition, int yPosition)
    {
        x = xPosition;
        y = yPosition;
    }

    public void TakeDamage(int damageAmount) => entityPower.ChangePowerCurrent(damageAmount);

    public void SetCombatState(bool combatState) => entityCombat.SetCombatState(combatState);

    public void AnimateTileBump(Vector3 _startingPosition, Vector3 _direction, Action _onContactCallback, Action _onCompleteCallback)
    {
        entityCombat.AnimateTileBump(_startingPosition, _direction, _onContactCallback, _onCompleteCallback);
    }

    public void KillEntity() => Die(); // This points to a separate method which can be overriden by the Enemy class to award XP

    protected virtual void Die()
    {
        EntityDied?.Invoke();
        KilledAtPosition?.Invoke(transform.position);
        DungeonManager.Instance.tileGrid[x, y].boundGameObject = null; // TODO Convert this to method within Dungeon Manager
        DungeonManager.Instance.tileGrid[x, y].MarkAsOccupied(false);
        Destroy(gameObject);
    }
}
}