using System;

namespace FMT
{
public class PlayerPower : EntityPower
{
    public static event Action PowerChanged;

    void OnEnable() 
    {
        PlayerXP.PlayerLeveledUp += RefillPower;
        PlayerFood.OnStarve += ChangePowerCurrent;
    }

    void OnDisable()
    {  
        PlayerXP.PlayerLeveledUp -= RefillPower;
        PlayerFood.OnStarve -= ChangePowerCurrent;
    }

    public void RefillPower(int maxPowerIncrease)
    {
        ChangePowerMax(maxPowerIncrease);
        SetPowerCurrent(powerMax);
    }

    protected override void CheckPower()
    {
        base.CheckPower();

        PowerChanged?.Invoke();
    }
}
}