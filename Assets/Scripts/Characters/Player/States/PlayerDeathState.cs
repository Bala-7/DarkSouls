using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathState : PlayerState
{
    private float currentTime = 0f;
    private float timeInState = 1f;

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }

    public override void Enter(ThirdPersonControllerMovement player)
    {
        player.PlayAnimation("Death");
        currentTime = 0f;
        player.Audio.PlayHitSound();
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= timeInState)
        {
            if (player.HasFinishedAnimation("Death")) 
            {
                // TODO: Wait 3s, then fade to black and reset game
            }
            
        }
        
        return null;
    }
}
