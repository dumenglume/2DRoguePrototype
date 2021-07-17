using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class GameManager : MonoBehaviour
{
    public enum GameState { menu, game, options, about };
    public GameState gameState = GameState.game;

    public bool gameHasStarted = false;

    public int playerPowerCurrent;
    public int playerPowerMax;
    public int playerXPCurrent;
    public int playerXPNext;
    public int playerGold;
    public int playerFoodCurrent;
    public int playerFoodMax;

    public int currentFloor;

    static GameManager instance;
    public static GameManager Instance => instance;

    [SerializeField] float generationSpeed = 0.0f;
    public float GenerationSpeed => generationSpeed;

    void Awake()
    {
        if   (instance != null && instance != this) { Destroy(this.gameObject); }
        else { instance = this; }
    }

    void OnEnable()
    {
        DungeonManager.dungeonComplete += StartGame;
    }

    void OnDisable()
    {
        DungeonManager.dungeonComplete -= StartGame;
    }

    void StartGame()
    {
        gameState      = GameState.game;
        gameHasStarted = true;
    }

    public void SavePlayerPowerCurrent(int amount) => playerPowerCurrent = amount;
    public void SavePlayerPowerMax(int amount) => playerPowerMax = amount;
    public void SavePlayerXPCurrent(int amount) => playerXPCurrent = amount;
    public void SavePlayerXPNext(int amount) => playerXPNext = amount;
    public void SavePlayerFoodCurrent(int amount) => playerGold = amount;
    public void SavePlayerFoodMax(int amount) => playerFoodMax = amount;
    public void SavePlayerGold(int amount) => playerFoodCurrent = amount;
    public void SaveCurrentFloor(int amount) => currentFloor = amount;

}
}