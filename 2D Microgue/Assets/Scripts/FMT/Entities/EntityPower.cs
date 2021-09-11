using System;
using UnityEngine;

namespace FMT
{
public class EntityPower : MonoBehaviour
{
    public static event Action PowerChanged;

    [SerializeField] protected IntReference initialPowerCurrent;
    [SerializeField] protected IntReference intitialPowerMax;
    [SerializeField] protected IntVariable powerCurrent;
    [SerializeField] protected IntVariable powerMax;
    public int PowerCurrent 
    { 
        get => powerCurrent.Value;

        set
        {
            powerCurrent.Value = value;
            CheckPower();
        }
    }
    public int PowerMax 
    { 
        get => powerMax.Value;

        set
        {
            powerMax.Value = value;
            CheckPower();
        }
    }

    Entity parentEntity; // TODO Find cleaner way of doing this

    public void BindEntity(Entity entity) => parentEntity = entity;

    public void SetPowerCurrent(int powerCurrentAmount) => PowerCurrent = powerCurrentAmount;

    public void ChangePowerCurrent(int powerCurrentAmount) => PowerCurrent += powerCurrentAmount;

    public void SetPowerMax(int powerMaxAmount) => PowerMax = powerMaxAmount;

    public void ChangePowerMax(int powerMaxAmount) => PowerMax += powerMaxAmount;

    protected virtual void CheckPower()
    {
        if (powerCurrent.Value <= 0)
        {
            powerCurrent.Value = 0;
            PowerDepleted();
        }

        else if (powerCurrent.Value > powerMax.Value)
        {
            powerCurrent.Value = powerMax.Value;
        }

        BroadcastPowerChanged();
    }

    protected virtual void PowerDepleted() => parentEntity.KillEntity();

    void BroadcastPowerChanged() => PowerChanged?.Invoke();
}
}