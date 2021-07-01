using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HB
{
public class HBGameManager : MonoBehaviour
{
    public enum HBGameState { menu, game, options, about };
    public static HBGameState gameState = HBGameState.menu;

    void OnEnable() 
    {
        HBItemSpawner.allItemsSpawned += StartGame;
    }

    void OnDisable()
    {
        HBItemSpawner.allItemsSpawned -= StartGame;
    }

    void StartGame()
    {
        gameState = HBGameState.game;
    }
}
}