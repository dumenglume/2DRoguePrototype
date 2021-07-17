using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class PlayerFood : MonoBehaviour
{
    public static event Action FoodAmountChanged;
    public static event Action<int> OnStarve;

    [SerializeField] int foodCurrent = 100;
    [SerializeField] int foodMax     = 100;
    [SerializeField] int foodReductionRate = -1;

    public int CurrentFood
    {
        get => foodCurrent;

        set
        {
            foodCurrent = value;
            FoodAmountChanged?.Invoke();

            if (foodCurrent <= 0)
            {
                OnStarve?.Invoke(foodReductionRate);
            }
        }
    }

    public int FoodMax => foodMax;

    void OnEnable()
    {
        Food.PickedUpFood += ChangeFoodAmount;
        PlayerMovement.OnMovementComplete += ReduceFood;
    }

    void OnDisable()
    {
        Food.PickedUpFood -= ChangeFoodAmount;
        PlayerMovement.OnMovementComplete -= ReduceFood;
    }

    public void SetFoodAmount(int foodAmount) => CurrentFood = foodAmount;
    
    public void ChangeFoodAmount(int foodAmount)
    {
        CurrentFood += foodAmount;
        print("Consumed " + foodAmount + " food");
    }

    void ReduceFood() => ChangeFoodAmount(foodReductionRate);

    public void SetFoodMaxAmount(int foodMaxAmount) => foodMax = foodMaxAmount;
    
    public void ChangeFoodMaxAmount(int foodMaxAmount) => foodMax += foodMaxAmount;
}
}