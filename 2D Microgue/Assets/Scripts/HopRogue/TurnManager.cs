using System;

namespace HopRogue
{
public static class TurnManager
{
    public static event Action PlayerTurn;
    public static event Action EnemyTurn;

    public enum Turn { player, enemies };
    static Turn _currentTurn = Turn.player;

    public enum PlayerState { normal, shoot, haste, wall };
    static PlayerState _playerState = PlayerState.normal;
    public static PlayerState PlayerSkill => _playerState;

    public static void ProceedToNextTurn()
    {
        if (_currentTurn == Turn.player)
        {
            _currentTurn = Turn.enemies;
            EnemyTurn?.Invoke();
        }

        else if (_currentTurn == Turn.enemies)
        {
            _currentTurn = Turn.player;
            PlayerTurn?.Invoke();
        }
    }

    public static void SetCurrentTurn(Turn thisTurn) => _currentTurn = thisTurn;

    public static bool IsPlayerTurn() => _currentTurn == Turn.player;

    public static bool IsEnemyTurn() => _currentTurn == Turn.enemies;
}
}