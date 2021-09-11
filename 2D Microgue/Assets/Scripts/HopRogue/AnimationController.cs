using System;
using System.Collections;
using UnityEngine;
using HopRogue.Entities;

namespace HopRogue
{
public static class AnimationController
{
    public static bool IsAnimating { get; private set; } = false;
    static float _animationDuration = 0.225f;
    static float _attackDirectionDistance = 0.5f;

    public static bool AnimationInProgress() => IsAnimating;

    public static IEnumerator AnimateMovement(Entity entity, Vector3Int positionChange)
    {
        IsAnimating = true;

        LeanTween.move(entity.gameObject, positionChange, _animationDuration).setEaseInOutQuad();
        yield return new WaitForSeconds(_animationDuration);

        IsAnimating = false;
    }

    public static IEnumerator AnimateProjectile(GameObject projectile, Vector3Int targetPosition, float distanceFromTarget, Action OnContact)
    {
        IsAnimating = true;

        float modifiedAnimationDuration = _animationDuration * distanceFromTarget;

        LeanTween.move(projectile.gameObject, targetPosition, modifiedAnimationDuration).setEaseInQuart();
        yield return new WaitForSeconds(modifiedAnimationDuration);

        OnContact();

        IsAnimating = false;
    }

    public static IEnumerator AnimateCombat(Actor attacker, Entity defender, Vector3Int startingPosition, Vector3Int attackDirection, Action<Actor, Entity> OnContact)
    {
        IsAnimating = true;

        LTSeq sequence = LeanTween.sequence();

        sequence.append(LeanTween.move(attacker.gameObject, startingPosition + (((Vector3) attackDirection) * _attackDirectionDistance), _animationDuration).setEaseInQuint());
        sequence.append( () => 
        {
            OnContact(attacker, defender);
        } );
        sequence.append(LeanTween.move(attacker.gameObject, startingPosition, _animationDuration).setEaseOutQuint());

        yield return new WaitForSeconds(_animationDuration * 2); // * 2 because separate animations play for moving and retracting

        IsAnimating = false;
    }
}
}