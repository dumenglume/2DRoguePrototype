using System;
using UnityEngine;

namespace FMT
{
public class EnemyPower : EntityPower
{
    public static event Action<int> EntityAwardedXP;

    protected override void PowerDepleted()
    {
        base.PowerDepleted();

        EntityAwardedXP?.Invoke(powerMax);

        Debug.Log($"{gameObject.name} awarded {powerMax} XP");
    }
}
}