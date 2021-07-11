using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class GameManager : MonoBehaviour
{
    public enum GameState { menu, game, options, about };
    public static GameState gameState = GameState.game;

    void StartGame()
    {
        gameState = GameState.game;
    }
}
}