using UnityEngine;
using UnityEngine.UI;

namespace HopRogue
{
public class UIManager : MonoBehaviour
{
    [SerializeField] Text _textMeshDebug;
    [SerializeField] Text _textMeshDebug2;

    [SerializeField] GameManager _gameManager;
    [SerializeField] World _world; // TODO Move to Start method

    HopRogue.Entities.Player _player;

    void OnEnable()
    {
        GameManager.OnPopulateUI += StartUI;
    }

    void OnDisable()
    {
        GameManager.OnPopulateUI -= StartUI;
    }

    void StartUI()
    {
        _player = _world.CurrentDungeon.Player;
    }

    void Update()
    {
        if (_player != null)
        {
            _textMeshDebug.text = $"HEALTH: {_player.Health}/{_player.HealthMax}\nSKILL: {_gameManager.PlayerState.ToString()}";
            _textMeshDebug2.text = $"ENEMIES: {_world.CurrentDungeon.Monsters.Count}\nRANGE: {_player.AttackRange.x}-{_player.AttackRange.y}";
        }
    }
}
}