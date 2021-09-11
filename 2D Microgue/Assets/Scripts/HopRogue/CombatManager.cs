using System;
using System.Collections.Generic;
using UnityEngine;
using HopRogue.Entities;

namespace HopRogue
{
public static class CombatManager
{
    public static bool IsTargetInLOS(Entity attacker, Entity targetActor) // TODO Clean this shit up and use interfaces instead?
    {
    /*
        int xDistance = Mathf.Abs(targetActor.WorldPosition.x - attacker.WorldPosition.x);
        int yDistance = Mathf.Abs(targetActor.WorldPosition.y - attacker.WorldPosition.y);

        bool xAligned = attacker.WorldPosition.x == targetActor.WorldPosition.x && yDistance >= attacker.ShotRange.x && yDistance <= attacker.ShotRange.y;
        bool yAligned = attacker.WorldPosition.y == targetActor.WorldPosition.y && xDistance >= attacker.ShotRange.x && xDistance <= attacker.ShotRange.y;

        bool targetActorNotAligned = !(xAligned || yAligned);

        if (targetActorNotAligned)
            return false;

        List<Vector3Int> path = attacker.GetPathToTarget(targetActor.WorldPosition, attacker.TargetEntity, CurrentDungeon, false);

        for (int i = 0; i < path.Count - 1; i++)
        {
            Actor blockingActor = CurrentDungeon.GetActorAt(path[i]);

            if (blockingActor != null)
                return false;
        }

    */
        return true;
    }
}
}