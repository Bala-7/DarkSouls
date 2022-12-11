using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGuardianAttackState : EnemyAttackState
{
    public override void Enter(Enemy enemy)
    {
        base.Enter(enemy);

        enemy.GetCurrentProjectile().ChasePlayer();
        // TODO: Launch bullet
    }

    public override EnemyState Update(Enemy enemy)
    {
        float currentStatePercentage = enemy.GetCurrentAnimationStateCompletionPercentage();
        enemy.LookPlayer();
        
        if (enemy.IsCurrentAnimation("Attack"))
        {
            if (currentStatePercentage > 0.99f)
            {
                return enemy.GetState(ENEMY_STATE_ID.IDLE);
            }
        }

        return null;
    }
}
