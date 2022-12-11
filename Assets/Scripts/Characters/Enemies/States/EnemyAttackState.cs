using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        _player = ThirdPersonControllerMovement.s;
        enemy.StopMovement();
        enemy.PlayAnimation("Attack");
    }

    public override EnemyState HandleInput(Enemy enemy, ref PlayerInput input)
    {
        throw new System.NotImplementedException();
    }

    public override EnemyState Update(Enemy enemy)
    {
        Debug.LogError("Executing Update() method on EnemyAttackState. This is not correct.");
        return null;
    }
}
