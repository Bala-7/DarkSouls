using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrkIdleState : EnemyIdleState
{
    public override EnemyState Update(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, _player.transform.position);
        if (distanceToPlayer < 5f)
        {
            return enemy.GetState(ENEMY_STATE_ID.WALK);
        }

        return null;
    }
}
