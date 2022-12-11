using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState : PlayerState
{

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }


    public override void Enter(ThirdPersonControllerMovement player)
    {
        player.EnableCurrentWeapons();
        player.PlayAnimation("Attack_A");
        player.Performance.PlayerAttackA();
        player.Audio.PlayAttackSound();
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        if (player.HasFinishedAnimation("Attack_A"))
            return new PlayerFreeState();

        return null;
    }


}
