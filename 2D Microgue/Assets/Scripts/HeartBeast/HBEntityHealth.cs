using System;
using UnityEngine;

namespace HB
{
public class HBEntityHealth : MonoBehaviour
{
    public static event Action HealthChanged;
    public static event Action EntityDied;
    public static event Action<Vector3> PositionOfDeath;

    [SerializeField] int healthCurrent = 1;
    [SerializeField] int healthMax = 1;
    public int HealthCurrent => healthCurrent;
    public int HealthMax => healthMax;

    public void SetHealthCurrent(int _healthCurrentAmount)
    {
        healthCurrent = _healthCurrentAmount;
        HealthChanged?.Invoke();
        CheckHealth();
    }

    public void ChangeHealthCurrent(int _healthCurrentAmount)
    {
        healthCurrent += _healthCurrentAmount;
        HealthChanged?.Invoke();
        CheckHealth();
    }

    public void SetHealthMax(int _healthMaxAmount)
    {
        healthMax = _healthMaxAmount;
        HealthChanged?.Invoke();
        ClampHealth();
    }

    public void ChangeHealthMax(int _healthMaxAmount)
    {
        healthMax += _healthMaxAmount;
        HealthChanged?.Invoke();
        ClampHealth();
    }

    void CheckHealth()
    {
        if (healthCurrent <= 0)
        {
            // Entity dies
            healthCurrent = 0;
            KillEntity();
        }

        else if (healthCurrent > healthMax)
        {
            // Entity reached max health
            ClampHealth();
        }
    }

    void ClampHealth()
    {
        if (healthCurrent > healthMax) { healthCurrent = healthMax; }
    }

    void KillEntity()
    {
        EntityDied?.Invoke();
        PositionOfDeath?.Invoke(transform.position);
        Destroy(gameObject);
    }
}
}