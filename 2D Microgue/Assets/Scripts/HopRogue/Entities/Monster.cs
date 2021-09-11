using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HopRogue.Entities
{
public class Monster : Actor, ITakeDamage
{
    [SerializeField] GameObject _projectilePrefab;

    public override List<Vector3Int> GetPathToTarget(Vector3Int targetPosition, Entity targetEntity, Dungeon dungeon, bool debug = false)
    {
        return base.GetPathToTarget(targetPosition, targetEntity, dungeon, debug);
    }

    public override IEnumerator Co_HandleActorTurn(Vector3Int targetPosition, Dungeon dungeon)
    {
        if (AttemptRangedAttack())
        {
            float distanceFromTarget = (WorldPosition - targetPosition).magnitude; // * Calculated so projectile easing speed remains constant and doesn't change based on distance

            GameObject projectileObject = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);

            EntityAudio.PlaySFXAttack();

            yield return AnimateProjectile(projectileObject, TargetEntity, distanceFromTarget, InflictRangedDamage);
            Destroy(projectileObject);

            EntityAudio.PlaySFXAttack();

            SetRemainingActionsToZero();

            yield break;
        }

        yield return base.Co_HandleActorTurn(targetPosition, dungeon);
    }

    protected override IEnumerator HandleEntityEncounter(Vector3Int entityEncounteredPosition, Entity entityEncountered)
    {
        if (entityEncountered == TargetEntity)
        {
            yield return AnimateCombat(this, entityEncountered, WorldPosition, entityEncounteredPosition, OnContactAction); // * Change ResolveCombat to other actions to test them
            SetRemainingActionsToZero();
        }

        yield return null;
    }

    protected bool AttemptRangedAttack()
    {
        int xDistance            = Mathf.Abs(TargetEntity.WorldPosition.x - this.WorldPosition.x);
        int yDistance            = Mathf.Abs(TargetEntity.WorldPosition.y - this.WorldPosition.y);

        bool xWithinRange        = xDistance >= this._attackRange.x && xDistance <= this._attackRange.y;
        bool yWithinRange        = yDistance >= this._attackRange.x && yDistance <= this._attackRange.y;

        bool xAligned            = this.WorldPosition.x == TargetEntity.WorldPosition.x;
        bool yAligned            = this.WorldPosition.y == TargetEntity.WorldPosition.y;

        bool targetIsWithinRange = xWithinRange && yWithinRange;
        bool targetIsAligned     = (xAligned && !yAligned) || (!xAligned && yAligned);

        if (targetIsAligned)
            Debug.Log($"{this.name} is aligned with {TargetEntity.name}");

        if (targetIsWithinRange)
            Debug.Log($"{this.name} is within range of {TargetEntity.name}");

        if (targetIsAligned && targetIsWithinRange)
        {
            Debug.Log($"{this.name} attacked {TargetEntity.name} from range");
            return true;    
        }

        Debug.Log($"{this.name} can't attack {TargetEntity.name} from ranged.");
        return false;
    }

    void InflictRangedDamage() => TargetEntity.TakeDamage(_attackDamage);
}
}