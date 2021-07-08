using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public static class CombatManager
{
    static Entity attacker;
    static Entity defender;
    static Vector3 directionOfAttack;

    public static void CombatEncounter(Entity _player, Entity _enemy, Vector3 _directionOfAttack)
    {
        bool playerAttacksFirst = _player.EntityCombat.PowerLevel > _enemy.EntityCombat.PowerLevel; // NOTE Can be changed to different conditions later

        attacker = playerAttacksFirst ? _player : _enemy;
        defender = playerAttacksFirst ? _enemy : _player;
        directionOfAttack = playerAttacksFirst ? _directionOfAttack : -_directionOfAttack;

        defender.EntityCombat.SetCombatState(true);
        attacker.EntityCombat.SetCombatState(true);
        attacker.EntityCombat.BumpAgainstTile(attacker.transform.position, directionOfAttack, CalculateDamage, CounterAttack);
    }

    static void CalculateDamage()
    {
        int damage = attacker.EntityCombat.AttackPower;

        defender.EntityHealth.ChangeHealthCurrent(-damage);

        Debug.Log(attacker.EntityName + " attacks " + defender.EntityName + " for " + attacker.EntityCombat.AttackPower + " damage");
    }

    public static void CounterAttack()
    {
        Vector3 directionOfCounterAttack = -directionOfAttack;
        Entity counterAttacker = defender;
        Entity counterDefender = attacker;

        attacker = counterAttacker;
        defender = counterDefender;

        attacker.EntityCombat.BumpAgainstTile(attacker.transform.position, directionOfCounterAttack, CalculateDamage, EndCombat);
    }

    static void EndCombat()
    {
        attacker.EntityCombat.SetCombatState(false);
        defender.EntityCombat.SetCombatState(false);
    }
}
}