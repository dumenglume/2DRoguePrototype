using System;

namespace FMT
{
public class EnemyPower : EntityPower
{
    public static event Action<int> EntityAwardedXP;

    protected override void PowerDepleted()
    {
        base.PowerDepleted();

        EntityAwardedXP?.Invoke(powerMax);
    }
}
}