using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEmptyState : PlayerState
{

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }


    public override void Enter(ThirdPersonControllerMovement player)
    {
        return;
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        return null;
    }


}
