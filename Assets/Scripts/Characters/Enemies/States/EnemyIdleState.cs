using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    

    public override void Enter(Enemy enemy)
    {
        _player = ThirdPersonControllerMovement.s;
        enemy.PlayAnimation("Idle");
        enemy.DisableCurrentWeapons();
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
