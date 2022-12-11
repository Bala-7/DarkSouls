using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected ThirdPersonControllerMovement _player;
    protected float speed = 5f;
    protected bool chasingPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChasePlayer() { chasingPlayer = true; }
}
