using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        _player = ThirdPersonControllerMovement.s;
        enemy.StopMovement();
        enemy.PlayAnimation("Hit");
        enemy.Performance.DealDamage();
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
