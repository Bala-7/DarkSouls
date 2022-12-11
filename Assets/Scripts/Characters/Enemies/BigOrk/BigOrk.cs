using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigOrk : MeeleeEnemy, IDamageableCharacter
{
    
    protected override void Start()
    {
        base.Start();
        //_navMeshAgent.SetDestination(_player.transform.position);
        DisableCurrentWeapons();

        _states.Add(ENEMY_STATE_ID.IDLE, new BigOrkIdleState());
        _states.Add(ENEMY_STATE_ID.ATTACK, new BigOrkAttackState());
        _states.Add(ENEMY_STATE_ID.WALK, new BigOrkWalkState());
        _states.Add(ENEMY_STATE_ID.HIT, new BigOrkHitState());
        _states.Add(ENEMY_STATE_ID.DEATH, new BigOrkDeathState());

        _enemyState = GetState(ENEMY_STATE_ID.IDLE);
        _enemyState.Enter(this);
    }

    public override void OnReceiveHit()
    {
        if (CanBeHit) 
        { 
            base.OnReceiveHit();
            bool alive = Performance.NextHitWillKill();
            if (alive)
                _enemyState = GetState(ENEMY_STATE_ID.HIT);
            else
                _enemyState = GetState(ENEMY_STATE_ID.DEATH);

            _enemyState.Enter(this);
        }
    }

}
