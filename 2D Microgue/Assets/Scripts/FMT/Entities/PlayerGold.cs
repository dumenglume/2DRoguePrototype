using System;
using UnityEngine;

namespace FMT
{
public class PlayerGold : MonoBehaviour
{
    public static event Action GoldAmountChanged;

    [SerializeField] int currentGold = 0;

    public int CurrentGold
    {
        get => currentGold;

        set
        {
            currentGold = value;
            GoldAmountChanged?.Invoke();
        }
    }

    void OnEnable() => Gold.PickedUpGold += ChangeGoldAmount;

    void OnDisable() => Gold.PickedUpGold -= ChangeGoldAmount;

    public void SetGoldAmount(int goldAmount) => CurrentGold = goldAmount;
    
    public void ChangeGoldAmount(int goldAmount)
    {
        CurrentGold += goldAmount;
        print("Picked up " + goldAmount + " gold");
    }
}
}