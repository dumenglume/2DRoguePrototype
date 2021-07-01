using System;
using UnityEngine;

namespace HB
{
public class HBEntityCombat : MonoBehaviour
{
    [SerializeField] protected float bumpDuration = 0.0f;
    public float BumpDuration => bumpDuration;

    [SerializeField] protected int attackPower = 0;
    public int AttackPower => attackPower;

    [SerializeField] protected int powerLevel = 0;
    public int PowerLevel => powerLevel;

    protected bool isInCombat = false;
    public bool IsInCombat { get { return isInCombat; } set { isInCombat = value; } }


    public void SetBumpDuration(float _timeInSeconds) { bumpDuration = _timeInSeconds; }
    public void SetAttackPower(int _attackPower) { attackPower = _attackPower; }
    public void SetPowerLevel(int _powerLevel) { powerLevel = _powerLevel; }
    public void SetCombatState(bool _isInCombat) { isInCombat = _isInCombat; }


    public void BumpAgainstTile(Vector3 _startingPosition, Vector3 _direction, Action _onContactCallback, Action _onCompleteCallback)
    {
        if (LeanTween.isTweening()) { return; }

        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.move(gameObject, _startingPosition + (_direction * 0.5f), bumpDuration).setEaseInQuint());
        sequence.append( () => 
        {
            _onContactCallback(); // Calls back to Combat Manager to calculate damage and any other on contact events
            // TriggerTile(_destinationTile);
        } );
        sequence.append(LeanTween.move(gameObject, _startingPosition, bumpDuration).setEaseOutCubic().setOnComplete( () => 
        { 
            _onCompleteCallback();
        } ));
    }
}
}