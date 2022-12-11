using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeState : PlayerState
{
    private float hInput = 0f;
    private float vInput = 0f;
    private bool run = false;

    public override PlayerState HandleInput(ThirdPersonControllerMovement player, ref NewPlayerInput input)
    {
        hInput = input.AxisStates[(int)INPUT_AXIS.LH].Item2;
        vInput = input.AxisStates[(int)INPUT_AXIS.LV].Item2;
        run = (input.GetButtonState(INPUT_BUTTONS.BUTTON_B) == BUTTON_STATE.HOLD) && player.Performance.HasEnoughStaminaForRun();
        
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_B) == BUTTON_STATE.RELEASE)
        {
            if (player.Performance.HasEnoughStaminaForRoll())
            {
                return new PlayerRollState();
            }
        }
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_RB) == BUTTON_STATE.RELEASE)
        {
            if (player.Performance.HasEnoughStaminaForAttackA())
            {
                return new PlayerAttackState();
            }
        }
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_R3) == BUTTON_STATE.RELEASE)
        {
            Enemy closestEnemy = DungeonGenerator.s.GetClosestEnemyToPlayer();
            Debug.Log("Locking enemy " + closestEnemy.name);
            if (!player.IsLocked())
            {
                player.LockEnemy(closestEnemy);
                UIManager.s.OnEnemyLocked(closestEnemy);
            }
            else 
            {
                player.UnlockEnemy();
                UIManager.s.OnEnemyUnlocked();
            }

            return this;
        }
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_X) == BUTTON_STATE.RELEASE)
        {
            return new PlayerUseItemState();
        }
        if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 >= 1)
        {
            player.Inventory.NextCurrentObject();
        }

        return this;
    }


    public override void Enter(ThirdPersonControllerMovement player)
    {
        player.EnableMovement();
        player.DisableCurrentWeapons();
    }

    // Update is called once per frame
    public override PlayerState Update(ThirdPersonControllerMovement player)
    {
        if (player.IsMovementEnabled)
        {
            player.MovePlayer(hInput, vInput, run);
            player.AnimatePlayer(run);
        }

        return this;
    }
}
