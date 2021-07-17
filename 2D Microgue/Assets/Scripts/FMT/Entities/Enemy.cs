using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class Enemy : Entity
{
    protected EnemyPower enemyPower;
    public EnemyPower EnemyPower => enemyPower;

    protected override void Awake()
    {
      base.Awake();
      enemyPower  = GetComponent<EnemyPower>();
    }

    protected override void SetEntityStats()
    {
        base.SetEntityStats();

        enemyPower.BindEntity(this);
        enemyPower.SetPowerMax(Random.Range(1, entityStatsReference.PowerMax));
        enemyPower.SetPowerCurrent(Random.Range(1, entityStatsReference.PowerCurrent));
    }

    public override void TakeDamage(int damageAmount) => enemyPower.ChangePowerCurrent(damageAmount);
}
}