using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        _player = ThirdPersonControllerMovement.s;
        enemy.SetNavMeshAgentDestination(_player.transform.position);
        enemy.PlayAnimation("Walk");
    }

    public override EnemyState HandleInput(Enemy enemy, ref PlayerInput input)
    {
        throw new System.NotImplementedException();
    }

    public override EnemyState Update(Enemy enemy)
    {
        return null;
    }
}
