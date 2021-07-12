using System;
using UnityEngine;

namespace FMT
{
public class PlayerGold : MonoBehaviour
{
    public static event Action GoldAmountChanged;

    [SerializeField] int currentGold = 0;
    public int CurrentGold => currentGold;

    void OnEnable() => Gold.PickedUpGold += ChangeGoldAmount;

    void OnDisable() => Gold.PickedUpGold -= ChangeGoldAmount;

    void SetGoldAmount(int goldAmount) => currentGold = goldAmount;
    
    void ChangeGoldAmount(int goldAmount)
    {
        currentGold += goldAmount;
        GoldAmountChanged?.Invoke();
        print("Picked up " + goldAmount + " gold");
    }
}
}