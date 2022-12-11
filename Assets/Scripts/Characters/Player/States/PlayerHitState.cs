using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : PlayerState
{
    private float currentTime = 0f;
    private float timeInState = 1f;

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }

    public override void Enter(ThirdPersonControllerMovement player)
    {
        player.PlayAnimation("Hit");
        currentTime = 0f;
        player.Audio.PlayHitSound();
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeInState)
        {
            if (player.HasFinishedAnimation("Hit"))
                return new PlayerFreeState();
        }
        
        return null;
    }
}
