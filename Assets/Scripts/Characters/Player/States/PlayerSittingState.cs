using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSittingState : PlayerState
{
    private const float MaxTimeInState = 5f;
    private float currentTime = 0f;

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }


    public override void Enter(ThirdPersonControllerMovement player)
    {
        currentTime = 0f;
        player.DisableMovement();
        player.PlayAnimation("SittingIdle");
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= MaxTimeInState)
            return new PlayerFreeState();

        return null;
    }


}
