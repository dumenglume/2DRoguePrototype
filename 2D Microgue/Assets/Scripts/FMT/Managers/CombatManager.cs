using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class CombatManager : MonoBehaviour
{
    public static Action<float, float, float> OnShakeCamera;

    Entity attacker;
    Entity defender;
    Vector3 directionOfAttack;

    void OnEnable()
    {
        PlayerMovement.OnTileInteraction += CombatEncounter;
        EntityHealth.EntityDied          += EndCombat;
    }

    void OnDisable()
    {
        PlayerMovement.OnTileInteraction -= CombatEncounter;
        EntityHealth.EntityDied          -= EndCombat;
    }

    public void CombatEncounter(Entity player, Entity enemy, Vector3 _directionOfAttack)
    {
        bool playerAttacksFirst = player.EntityCombat.PowerLevel > enemy.EntityCombat.PowerLevel; // TODO Change this to "if player has first strike perk" once implemented

        attacker = playerAttacksFirst ? player : enemy;
        defender = playerAttacksFirst ? enemy : player;
        directionOfAttack = playerAttacksFirst ? _directionOfAttack : -_directionOfAttack;

        defender.SetCombatState(true);
        attacker.SetCombatState(true);
        attacker.AnimateTileBump(attacker.transform.position, directionOfAttack, InflictDamage, CounterAttack);
    }

    void InflictDamage()
    {
        int damage = attacker.EntityCombat.AttackPower;

        defender.TakeDamage(-damage);

        Debug.Log(attacker.EntityName + " attacks " + defender.EntityName + " for " + attacker.EntityCombat.AttackPower + " damage");

        OnShakeCamera?.Invoke(0.05f, 0.25f, 0.9f); // TODO Move this to its own method
    }

    public void CounterAttack()
    {
        Vector3 directionOfCounterAttack = -directionOfAttack;
        Entity counterAttacker = defender;
        Entity counterDefender = attacker;

        attacker = counterAttacker;
        defender = counterDefender;

        if (attacker == null) { return; }

        attacker.EntityCombat.AnimateTileBump(attacker.transform.position, directionOfCounterAttack, InflictDamage, EndCombat);
    }

    void EndCombat()
    {
        attacker.EntityCombat.SetCombatState(false);
        defender.EntityCombat.SetCombatState(false);
    }
}
}