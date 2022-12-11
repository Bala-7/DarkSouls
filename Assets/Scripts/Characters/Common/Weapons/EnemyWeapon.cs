using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        DamageReceiver playerDamageReceiver = other.GetComponent<DamageReceiver>();

        if (!System.Object.ReferenceEquals(playerDamageReceiver, null))
        {
            if (playerDamageReceiver.IsPlayer())
                playerDamageReceiver.OnReceiveHit();
        }
    }
}
