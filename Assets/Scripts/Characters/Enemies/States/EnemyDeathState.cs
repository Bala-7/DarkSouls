using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public override void Enter(Enemy enemy)
    {
        _player = ThirdPersonControllerMovement.s;
        enemy.SpawnSouls();
        enemy.StopMovement();
        enemy.PlayAnimation("Death");
        enemy.Die();
        if(enemy.Type.Equals(ENEMY_TYPE.BOSS))
            EventsManager.instance.NotifyEvent(GAME_EVENTS.BOSS_DEFEAT);
        EventsManager.instance.NotifyEvent(ENEMY_EVENTS.ENEMY_DEAD, enemy);
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
