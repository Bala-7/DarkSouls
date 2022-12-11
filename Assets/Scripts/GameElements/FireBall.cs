using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    private float currentTime = 0;
    private float lifeTime = 4f;
    private float force = 1f;

    private void Awake()
    {
        _player = ThirdPersonControllerMovement.s;
    }

    // Update is called once per frame
    void Update()
    {
        if (chasingPlayer) 
        {
            currentTime += Time.deltaTime;
            if (currentTime >= lifeTime)
                Destroy(this.gameObject);
            else 
            {
                Vector3 playerDirection = (_player.transform.position - transform.position).normalized;
                GetComponent<Rigidbody>().AddForce(playerDirection * force, ForceMode.Force);
            }
        }
    }
}
