using UnityEngine;

namespace HB {
public abstract class HBEntity : MonoBehaviour
{
    protected HBEntityCombat entityCombat;
    public HBEntityCombat EntityCombat => entityCombat;

    protected HBEntityHealth entityHealth;
    public HBEntityHealth EntityHealth => entityHealth;

    [SerializeField] protected HBEntityStats entityStats;

    [SerializeField] string entityName;
    public string EntityName => entityName;

    protected virtual void Awake() 
    {
        entityCombat   = GetComponent<HBEntityCombat>();
        entityHealth   = GetComponent<HBEntityHealth>();
    }

    protected virtual void Start()
    {
        SetStats();
    }

    protected virtual void SetStats()
    {
        entityName = entityStats.Name;
        entityHealth.SetHealthMax(entityStats.HealthMax);
        entityHealth.SetHealthCurrent(entityStats.HealthMax);
        entityCombat.SetBumpDuration(entityStats.BumpDuration);
        entityCombat.SetAttackPower(entityStats.AttackPower);
        entityCombat.SetPowerLevel(entityStats.PowerLevel);
    }
}
}