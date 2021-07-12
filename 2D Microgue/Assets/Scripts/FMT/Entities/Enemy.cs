using UnityEngine;

namespace FMT
{
public class Enemy : Entity
{
    protected override void SetEntityStats()
    {
        base.SetEntityStats();

        entityPower.SetPowerMax(Random.Range(1, entityStatsReference.PowerMax));
        entityPower.SetPowerCurrent(Random.Range(1, entityStatsReference.PowerCurrent));
    }
}
}