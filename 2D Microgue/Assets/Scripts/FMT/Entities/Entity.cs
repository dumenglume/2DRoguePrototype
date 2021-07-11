using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public abstract class Entity : MonoBehaviour
{
    protected EntityCombat entityCombat;
    public EntityCombat EntityCombat => entityCombat;

    protected EntityHealth entityHealth;
    public EntityHealth EntityHealth => entityHealth;

    [Header("Entity Settings")]
    [SerializeField] string entityName;
    public string EntityName => entityName;
    [SerializeField] protected EntityStats entityStatsReference;

    protected int x = 0;
    protected int y = 0;

    protected virtual void Awake() 
    {
        entityCombat   = GetComponent<EntityCombat>();
        entityHealth   = GetComponent<EntityHealth>();
    }

    protected void Start()
    {
        SetEntityStats();
        SetEntityPosition((int) transform.position.x, (int) transform.position.y);
    }

    protected virtual void SetEntityStats()
    {
        entityName = entityStatsReference.Name;
        entityHealth.SetHealthMax(entityStatsReference.HealthMax);
        entityHealth.SetHealthCurrent(entityStatsReference.HealthCurrent);
        entityCombat.SetBumpDuration(entityStatsReference.BumpDuration);
        entityCombat.SetAttackPower(entityStatsReference.AttackPower);
        entityCombat.SetPowerLevel(entityStatsReference.PowerLevel);
    }

    public void SetEntityPosition(int xPosition, int yPosition)
    {
        x = xPosition;
        y = yPosition;
    }

    public void TakeDamage(int damageAmount) => entityHealth.ChangeHealthCurrent(damageAmount);

    public void SetCombatState(bool combatState) => entityCombat.SetCombatState(combatState);

    public void AnimateTileBump(Vector3 _startingPosition, Vector3 _direction, Action _onContactCallback, Action _onCompleteCallback)
    => entityCombat.AnimateTileBump(_startingPosition, _direction, _onContactCallback, _onCompleteCallback);


}
}