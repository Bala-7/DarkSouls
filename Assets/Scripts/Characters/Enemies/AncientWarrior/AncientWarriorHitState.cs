using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWarriorHitState : EnemyHitState
{
    public override EnemyState Update(Enemy enemy)
    {
        if (enemy.IsCurrentAnimation("Hit"))
        {
            float currentStatePercentage = enemy.GetCurrentAnimationStateCompletionPercentage();
            if (currentStatePercentage > 0.99f)
            {
                return enemy.GetState(ENEMY_STATE_ID.IDLE);
            }

        }
        return null;
    }
}
