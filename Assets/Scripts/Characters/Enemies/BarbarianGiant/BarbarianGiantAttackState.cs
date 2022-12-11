using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianGiantAttackState : EnemyAttackState
{
    public override EnemyState Update(Enemy enemy)
    {
        float currentStatePercentage = enemy.GetCurrentAnimationStateCompletionPercentage();
        
        if (enemy.IsCurrentAnimation("Attack"))
        {
            if (currentStatePercentage > 0.42f && currentStatePercentage <= 0.6f)
            {
                enemy.EnableCurrentWeapons();
            }
            else if (currentStatePercentage > 0.6f && currentStatePercentage <= 0.99f)
            {
                enemy.DisableCurrentWeapons();
            }
            else if (currentStatePercentage > 0.99f)
            {
                return enemy.GetState(ENEMY_STATE_ID.WALK);
            }
        }

        return null;
    }
}
