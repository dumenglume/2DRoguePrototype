using System;
using UnityEngine;

namespace FMT
{
public class EntityPower : MonoBehaviour
{
    public static event Action PowerChanged;

    [SerializeField] int powerCurrent = 1;
    [SerializeField] int powerMax = 1;
    public int PowerCurrent => powerCurrent;
    public int PowerMax => powerMax;
    Entity parentEntity; // TODO Find cleaner way of doing this

    [SerializeField] TextMesh powerText; // TODO Find a way to separate this into its own class

    public void BindEntity(Entity entity) => parentEntity = entity;

    public void SetPowerCurrent(int powerCurrentAmount)
    {
        powerCurrent = powerCurrentAmount;
        PowerChanged?.Invoke();
        CheckPower();
        UpdatePowerText();
    }

    public void ChangePowerCurrent(int powerCurrentAmount)
    {
        powerCurrent += powerCurrentAmount;
        PowerChanged?.Invoke();
        CheckPower();
        UpdatePowerText();
    }

    public void SetPowerMax(int powerMaxAmount)
    {
        powerMax = powerMaxAmount;
        PowerChanged?.Invoke();
        ClampPower();
        UpdatePowerText();
    }

    public void ChangePowerMax(int powerMaxAmount)
    {
        powerMax += powerMaxAmount;
        PowerChanged?.Invoke();
        ClampPower();
        UpdatePowerText();
    }

    void CheckPower()
    {
        if (powerCurrent <= 0)
        {
            powerCurrent = 0;
            parentEntity.KillEntity();
        }

        else if (powerCurrent > powerMax)
        {
            ClampPower();
        }
    }

    void ClampPower() { if (powerCurrent > powerMax) { powerCurrent = powerMax; } }

    void UpdatePowerText() => powerText.text = powerCurrent.ToString();
}
}