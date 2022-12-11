using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsFXBehaviour : MonoBehaviour
{
    private Transform _player;
    private float _speed = 5.0f;

    private void Awake()
    {
        _player = ThirdPersonControllerMovement.s.transform;
    }
    

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = _player.position + Vector3.up;
        Vector3 playerDirection = playerPosition - transform.position;

        if (playerDirection.magnitude <= 0.5f)
            StartCoroutine("DelayedDestroyCoroutine");
        transform.position += _speed * playerDirection.normalized * Time.deltaTime;

    }

    private IEnumerator DelayedDestroyCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
