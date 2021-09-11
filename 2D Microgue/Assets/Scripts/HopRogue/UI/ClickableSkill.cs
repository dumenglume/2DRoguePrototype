using System;
using UnityEngine;

namespace HopRogue
{
public class ClickableSkill : MonoBehaviour
{
    public static event Action<bool, int> OnSkillClicked;

    [SerializeField] bool _skillIsActive;
    [SerializeField] int _stateIndex;

    void OnEnable() => GameManager.ResetButtons += ResetSkill;

    void OnDisable() => GameManager.ResetButtons -= ResetSkill;

    void OnMouseDown()
    {
        _skillIsActive = !_skillIsActive;
        OnSkillClicked?.Invoke(_skillIsActive, _stateIndex);
    }

    void ResetSkill() => _skillIsActive = false;
}
}