using UnityEngine;

namespace FMT
{
public class EntityPower : MonoBehaviour
{
    [SerializeField] protected int powerCurrent = 10;
    [SerializeField] protected int powerMax = 10;
    public int PowerCurrent 
    { 
        get => powerCurrent;

        set
        {
            powerCurrent = value;
            CheckPower();
        }
    }
    public int PowerMax 
    { 
        get => powerMax;

        set
        {
            powerMax = value;
            CheckPower();
        }
    }

    [SerializeField] TextMesh powerSpriteText; // TODO Find a way to separate this into its own class
    Entity parentEntity; // TODO Find cleaner way of doing this

    void OnEnable()
    {
        PlayerXP.PlayerLeveledUp += RefillPower;
    }

    void OnDisable()
    {
        PlayerXP.PlayerLeveledUp -= RefillPower;
    }

    public void BindEntity(Entity entity) => parentEntity = entity;

    public void SetPowerCurrent(int powerCurrentAmount) => PowerCurrent = powerCurrentAmount;

    public void ChangePowerCurrent(int powerCurrentAmount) => PowerCurrent += powerCurrentAmount;

    public void SetPowerMax(int powerMaxAmount) => PowerMax = powerMaxAmount;

    public void ChangePowerMax(int powerMaxAmount) => PowerMax += powerMaxAmount;

    public void RefillPower(int maxPowerIncrease)
    {
        ChangePowerMax(maxPowerIncrease);
        SetPowerCurrent(powerMax);
    }

    protected virtual void CheckPower()
    {
        if (powerCurrent <= 0)
        {
            powerCurrent = 0;
            PowerDepleted();
        }

        else if (powerCurrent > powerMax)
        {
            powerCurrent = powerMax;
        }

        UpdatePowerSpriteText();
    }

    protected virtual void PowerDepleted() => parentEntity.KillEntity();

    void UpdatePowerSpriteText() => powerSpriteText.text = powerCurrent.ToString();
}
}