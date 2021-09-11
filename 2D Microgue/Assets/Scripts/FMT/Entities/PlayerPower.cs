namespace FMT
{
public class PlayerPower : EntityPower
{
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
        SetPowerCurrent(powerMax.Value);
    }
}
}