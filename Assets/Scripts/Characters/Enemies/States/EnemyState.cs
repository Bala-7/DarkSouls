using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected ThirdPersonControllerMovement _player;

    public abstract EnemyState HandleInput(Enemy enemy, ref PlayerInput input);

    public abstract void Enter(Enemy enemy);

    public abstract EnemyState Update(Enemy enemy);

}
