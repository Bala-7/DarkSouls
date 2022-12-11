using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestGuardian : RangeEnemy
{
    // Start is called before the first frame update
    protected override void Start()
    {
        _navMeshAgent.SetDestination(_player.transform.position);
        DisableCurrentWeapons();

        _states.Add(ENEMY_STATE_ID.IDLE, new ForestGuardianIdleState());
        _states.Add(ENEMY_STATE_ID.ATTACK, new ForestGuardianAttackState());
        _states.Add(ENEMY_STATE_ID.CHARGE, new ForestGuardianChargeState());
        _states.Add(ENEMY_STATE_ID.HIT, new ForestGuardianHitState());
        _states.Add(ENEMY_STATE_ID.DEATH, new ForestGuardianDeathState());

        _enemyState = GetState(ENEMY_STATE_ID.IDLE);
        _enemyState.Enter(this);

        _projectileInstances = new List<Projectile>();

        base.Start();
    }

    public override void OnReceiveHit()
    {
        if (CanBeHit)
        {
            base.OnReceiveHit();
            DestroyProjectileInstances();
            bool aliveAfterHit = Performance.IsAliveAfterNextHit();
            if (aliveAfterHit)
                _enemyState = GetState(ENEMY_STATE_ID.HIT);
            else
                _enemyState = GetState(ENEMY_STATE_ID.DEATH);
        
            _enemyState.Enter(this);
        }
    }

}
