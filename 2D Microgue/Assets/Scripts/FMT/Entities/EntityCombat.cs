using System;
using UnityEngine;

namespace FMT
{
public class EntityCombat : MonoBehaviour
{
    [SerializeField] protected float bumpDuration = 0.0f;
    public float BumpDuration => bumpDuration;

    protected bool isAnimatingCombat = false;
    public bool IsAnimatingCombat => isAnimatingCombat;


    public void SetBumpDuration(float timeInSeconds)   { bumpDuration = timeInSeconds; }
    public void SetCombatState(bool isAnimatingCombat) { this.isAnimatingCombat = isAnimatingCombat; }


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