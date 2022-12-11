using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGuardianChargeState : EnemyChargeState
{
    private float _chargeTime = 3f;

    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);
        Vector3 spawnPosition = enemy.transform.position + (Vector3.up * 2.5f) + (enemy.transform.forward);
        Projectile p = Object.Instantiate(enemy.ProjectilePrefab, spawnPosition, Quaternion.identity);
        enemy.AddToProjectiles(p);
    }

    public override EnemyState Update(Enemy enemy)
    {
        enemy.LookPlayer();
        
        currentTimer += Time.deltaTime;
        if (currentTimer >= _chargeTime)
        {
            return enemy.GetState(ENEMY_STATE_ID.ATTACK);
        }

        return null;
    }

}
