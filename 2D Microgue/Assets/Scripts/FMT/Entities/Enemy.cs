using System;
using UnityEngine;
using Random = UnityEngine.Random;

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