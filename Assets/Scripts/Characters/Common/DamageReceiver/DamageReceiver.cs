using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    private GameCharacter _thisGameCharacter;

    private void Awake()
    {
        _thisGameCharacter = transform.parent.GetComponent<GameCharacter>();
    }

    public void OnReceiveHit()
    {
        _thisGameCharacter.OnReceiveHit();

    }

    public bool IsEnemy()
    {
        Enemy enemy = transform.parent.GetComponent<Enemy>();
        return (!ReferenceEquals(enemy, null));
    }

    public bool IsPlayer()
    {
        ThirdPersonControllerMovement player = transform.parent.GetComponent<ThirdPersonControllerMovement>();
        return (!ReferenceEquals(player, null));
    }

}
