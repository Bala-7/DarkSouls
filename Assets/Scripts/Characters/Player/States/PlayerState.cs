using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    public abstract PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input);

    public abstract void Enter(ThirdPersonControllerMovement player);

    public abstract PlayerState Update(ThirdPersonControllerMovement player);

}
