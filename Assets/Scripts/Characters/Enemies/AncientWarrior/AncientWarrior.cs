using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientWarrior : MeeleeEnemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //_navMeshAgent.SetDestination(_player.transform.position);
        DisableCurrentWeapons();

        _states.Add(ENEMY_STATE_ID.IDLE, new AncientWarriorIdleState());
        _states.Add(ENEMY_STATE_ID.ATTACK, new AncientWarriorAttackState());
        _states.Add(ENEMY_STATE_ID.WALK, new AncientWarriorWalkState());
        _states.Add(ENEMY_STATE_ID.HIT, new AncientWarriorHitState());
        _states.Add(ENEMY_STATE_ID.DEATH, new AncientWarriorDeathState());

        _enemyState = GetState(ENEMY_STATE_ID.IDLE);
        _enemyState.Enter(this);
    }

    public override void OnReceiveHit()
    {
        if (CanBeHit) 
        { 
            base.OnReceiveHit();
            bool aliveAfterHit = Performance.IsAliveAfterNextHit();
            if (aliveAfterHit)
                _enemyState = GetState(ENEMY_STATE_ID.HIT);
            else
                _enemyState = GetState(ENEMY_STATE_ID.DEATH);
       
            _enemyState.Enter(this);
        }
    }

}
