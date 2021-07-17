using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class PlayerXP : MonoBehaviour
{
    public static event Action<int> PlayerLeveledUp; // TODO Pass current level?
    public static event Action PlayerXPChanged;

    [SerializeField] int currentXP = 0;

    public int CurrentXP
    { 
        get => currentXP;

        set 
        {
            Debug.Log($"Current XP: {currentXP} + Value: {value} / Next Level: {nextLevel}");

            if (value >= nextLevel) { LevelUp(value); }

            else { currentXP = value; }

            PlayerXPChanged?.Invoke();
        }
    }

    [SerializeField] int nextLevel = 20;
    public int NextLevel { get => nextLevel; set => nextLevel = value; }

    [SerializeField] int xpRequirementMultiplier = 5;
    const int increasePerLevel = 1;

    void OnEnable() => EnemyPower.EntityAwardedXP += ChangeXPAmount;

    void OnDisable() => EnemyPower.EntityAwardedXP -= ChangeXPAmount;

    public void SetXPAmount(int xpAmount) => CurrentXP = xpAmount;

    public void ChangeXPAmount(int xpAmount) => CurrentXP += xpAmount;

    public void SetNextXP(int xpAmount) => nextLevel = xpAmount;

    void LevelUp(int value)
    {
        int leftoverXP = (CurrentXP + value) - nextLevel;
        CurrentXP = leftoverXP;
        nextLevel += xpRequirementMultiplier;
        PlayerLeveledUp?.Invoke(increasePerLevel);
    }
}
}