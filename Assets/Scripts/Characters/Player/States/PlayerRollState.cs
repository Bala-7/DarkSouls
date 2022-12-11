using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRollState : PlayerState
{

    private Vector3 _rollDirection;

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        return null;
    }


    public override void Enter(ThirdPersonControllerMovement player)
    {
        float hInput = player.LastInput.AxisStates[(int)INPUT_AXIS.LH].Item2;
        float vInput = player.LastInput.AxisStates[(int)INPUT_AXIS.LV].Item2;

        Vector3 direction = player.GetBodyForward();    // The player will roll to front if there is no input
        if (hInput != 0 && vInput != 0)
        {
            Vector3 camFlatFwd = Vector3.Scale(player.Camera.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 flatRight = new Vector3(player.Camera.transform.right.x, 0, player.Camera.transform.right.z);

            Vector3 m_CharForward = Vector3.Scale(camFlatFwd, new Vector3(1, 0, 1)).normalized;
            Vector3 m_CharRight = Vector3.Scale(flatRight, new Vector3(1, 0, 1)).normalized;


            direction = vInput * m_CharForward + hInput * m_CharRight;
            float angle = Vector3.SignedAngle(Vector3.Scale(player.GetBodyForward(), new Vector3(1, 0, 1)), Vector3.Scale(direction, new Vector3(1, 0, 1)), Vector3.up);
            player.TPC.transform.Rotate(Vector3.up, angle);
        }
        
        _rollDirection = Vector3.Scale(direction, new Vector3(1, 0, 1));


        player.PlayAnimation("Roll");
        player.Performance.Roll();
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        player.MovePlayerRolling(_rollDirection);

        if (player.HasFinishedAnimation("Roll"))
            return new PlayerFreeState();

        return null;
    }


}

