using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private float _physicalAttack = 10f;
    public float PhysicalAttack { get { return _physicalAttack; } }
    private float _magicAttack = 0f;
    public float MagicAttack { get { return _magicAttack; } }

    private float _weight = 5f;
    public float Weight { get { return _weight; } }

    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver enemyDamageReceiver = other.GetComponent<DamageReceiver>();

        if (!System.Object.ReferenceEquals(enemyDamageReceiver, null))
        {
            if(enemyDamageReceiver.IsEnemy())
                enemyDamageReceiver.OnReceiveHit();
        }
    }
}
