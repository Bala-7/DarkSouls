using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        _enemyState = GetState(ENEMY_STATE_ID.IDLE);
    }

    protected override void Update()
    {
        if (alive)
        {
            EnemyState newState = _enemyState.Update(this);
            if (!ReferenceEquals(newState, null))
            {
                _enemyState = newState;
                _enemyState.Enter(this);
            }
        }
    }

    protected void DestroyProjectileInstances()
    {
        for (int i = 0; i < _projectileInstances.Count; ++i)
        {
            Destroy(_projectileInstances[i].gameObject);
        }
        _projectileInstances.Clear();
    }

}
