using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianGiant : MeeleeEnemy, IDamageableCharacter
{
    
    protected override void Start()
    {
        base.Start();
        //_navMeshAgent.SetDestination(_player.transform.position);
        DisableCurrentWeapons();

        _states.Add(ENEMY_STATE_ID.IDLE, new BarbarianGiantIdleState());
        _states.Add(ENEMY_STATE_ID.ATTACK, new BarbarianGiantAttackState());
        _states.Add(ENEMY_STATE_ID.WALK, new BarbarianGiantWalkState());
        _states.Add(ENEMY_STATE_ID.HIT, new BarbarianGiantHitState());
        _states.Add(ENEMY_STATE_ID.DEATH, new BarbarianGiantDeathState());

        _enemyState = GetState(ENEMY_STATE_ID.IDLE);
        _enemyState.Enter(this);
    }

    public override void OnReceiveHit()
    {
        if (CanBeHit)
        {
            bool aliveAfterHit = Performance.IsAliveAfterNextHit();
            if (aliveAfterHit)
                _enemyState = GetState(ENEMY_STATE_ID.HIT);
            else
                _enemyState = GetState(ENEMY_STATE_ID.DEATH);
        
            _enemyState.Enter(this);
        }
    }

}
