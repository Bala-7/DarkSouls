using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWarriorWalkState : EnemyWalkState
{
    public override EnemyState Update(Enemy enemy)
    {
        float distanceToPlayer = Vector3.Distance(enemy.transform.position, _player.transform.position);
        if (distanceToPlayer <= 1f && _player.IsAlive())
        {
            return enemy.GetState(ENEMY_STATE_ID.ATTACK);
        }
        else
        {
            enemy.SetNavMeshAgentDestination(_player.transform.position);
        }
        return null;
    }

}
