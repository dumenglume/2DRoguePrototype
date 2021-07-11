﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FMT
{
public class PlayerGold : MonoBehaviour
{
    public static event Action GoldAmountChanged;

    [SerializeField] int currentGold = 0;
    public int CurrentGold { get { return currentGold; } set { currentGold = value; } }

    void OnEnable()
    {
        // HBTileGold.PickedUpGold += ChangeGoldAmount;
    }

    void OnDisable()
    {
        // HBTileGold.PickedUpGold += ChangeGoldAmount;
    }

    void ChangeGoldAmount()
    {
        int goldAmount = Random.Range(10, 15);

        print("Picked up " + goldAmount + " gold");

        currentGold += goldAmount;

        GoldAmountChanged?.Invoke();
    }
}
}