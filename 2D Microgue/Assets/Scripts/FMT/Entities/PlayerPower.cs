using System;

namespace FMT
{
public class PlayerPower : EntityPower
{
    public static event Action PowerChanged;

    protected override void CheckPower()
    {
        base.CheckPower();

        PowerChanged?.Invoke();
    }
}
}