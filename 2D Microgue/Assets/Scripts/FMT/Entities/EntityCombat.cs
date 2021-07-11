using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class EntityCombat : MonoBehaviour
{
    [SerializeField] protected float bumpDuration = 0.0f;
    public float BumpDuration => bumpDuration;

    [SerializeField] protected int attackPower = 0;
    public int AttackPower => attackPower;

    [SerializeField] protected int powerLevel = 0;
    public int PowerLevel => powerLevel;

    protected bool isInCombat = false;
    public bool IsInCombat => isInCombat;


    public void SetBumpDuration(float timeInSeconds) { bumpDuration = timeInSeconds; }
    public void SetAttackPower(int attackPower)      { this.attackPower = attackPower; }
    public void SetPowerLevel(int powerLevel)        { this.powerLevel = powerLevel; }
    public void SetCombatState(bool isInCombat)      { this.isInCombat = isInCombat; }


    public void AnimateTileBump(Vector3 _startingPosition, Vector3 _direction, Action _onContactCallback, Action _onCompleteCallback)
    {
        if (LeanTween.isTweening()) { return; }

        LTSeq sequence = LeanTween.sequence();
        sequence.append(LeanTween.move(gameObject, _startingPosition + (_direction * 0.5f), bumpDuration).setEaseInQuint());
        sequence.append( () => 
        {
            _onContactCallback(); // Triggered when entity is fully bumped into target
        } );
        sequence.append(LeanTween.move(gameObject, _startingPosition, bumpDuration).setEaseInOutQuint().setOnComplete( () => 
        { 
            _onCompleteCallback();
        } ));
    }
}
}