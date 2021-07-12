using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMT
{
public class CombatManager : MonoBehaviour
{
    public static Action<float, float, float> ShakeCamera;

    Entity playerEntity;
    Entity enemyEntity;
    Entity defendingEntity;
    int playerDamage;
    int enemyDamage;
    int damageToInflict;
    Vector3 direction;

    void OnEnable()
    {
        PlayerMovement.OnTileInteraction += CombatEncounter;
        Entity.EntityDied                += EndCombat;
    }

    void OnDisable()
    {
        PlayerMovement.OnTileInteraction -= CombatEncounter;
        Entity.EntityDied                -= EndCombat;
    }

    public void CombatEncounter(Entity player, Entity enemy, Vector3 directionOfAttack)
    {
        playerEntity = player;
        playerDamage = playerEntity.EntityPower.PowerCurrent;
        playerEntity.SetCombatState(true);

        enemyEntity = enemy;
        enemyDamage = enemyEntity.EntityPower.PowerCurrent;
        enemyEntity.SetCombatState(true);

        direction = directionOfAttack;

        bool enemyCanAttack = true;

        if (enemyCanAttack) { EnemyAttack(); }

        else { PlayerAttack(); }
    }

    void EnemyAttack()
    {
        defendingEntity = playerEntity;
        damageToInflict = enemyDamage;
        enemyEntity.AnimateTileBump(enemyEntity.transform.position, -direction, InflictDamage, PlayerAttack);
    }

    void PlayerAttack()
    {
        if (playerEntity == null) { return; }

        defendingEntity = enemyEntity;
        damageToInflict = playerDamage;
        playerEntity.AnimateTileBump(playerEntity.transform.position, direction, InflictDamage, EndCombat);   
    }

    void InflictDamage()
    {
        defendingEntity.TakeDamage(-damageToInflict);
        ShakeCamera?.Invoke(0.05f, 0.25f, 0.9f); // TODO Move this to its own method or make camera shake a static class
    }

    void EndCombat()
    {
        playerEntity.EntityCombat.SetCombatState(false);
        enemyEntity.EntityCombat.SetCombatState(false);
    }
}
}