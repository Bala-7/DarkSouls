using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItemState : PlayerState
{
    public override void Enter(ThirdPersonControllerMovement player)
    {
        player.PlayAnimation("UseItem");
        player.Inventory.UseCurrentItem();
    }

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }

    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        if (player.HasFinishedAnimation("UseItem"))
            return new PlayerFreeState();

        return null;
    }
}
