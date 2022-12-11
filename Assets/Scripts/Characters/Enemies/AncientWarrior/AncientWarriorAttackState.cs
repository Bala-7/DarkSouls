using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWarriorAttackState : EnemyAttackState
{
    public override EnemyState Update(Enemy enemy)
    {
        float currentStatePercentage = enemy.GetCurrentAnimationStateCompletionPercentage();
        if (enemy.IsCurrentAnimation("Attack"))
        {
            if (currentStatePercentage > 0.35f && currentStatePercentage <= 0.45f)
            {
                enemy.EnableCurrentWeapons();
            }
            else if (currentStatePercentage > 0.45f && currentStatePercentage <= 0.99f)
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
