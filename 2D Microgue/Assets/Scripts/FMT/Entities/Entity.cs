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
        entityHealth.SetHealthCurrent(entityStatsReference.HealthMax);
        entityCombat.SetBumpDuration(entityStatsReference.BumpDuration);
        entityCombat.SetAttackPower(entityStatsReference.AttackPower);
        entityCombat.SetPowerLevel(entityStatsReference.PowerLevel);
    }

    public void SetEntityPosition(int xPosition, int yPosition)
    {
        x = xPosition;
        y = yPosition;
    }
}
}