using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArmorState : UIState
{
    private ArmorUI _armorUI;

    public UIArmorState(ArmorUI armorUI)
    {
        _armorUI = armorUI;
    }

    public override UIState OnEnter()
    {
        _armorUI.OnArmorUISelected();
        return this;
    }

    public override UIState OnExit()
    {
        _armorUI.OnArmorUIUnselected();
        return this;
    }

    public override UIState OnInput(NewPlayerInput input)
    {
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_LB) == BUTTON_STATE.RELEASE)
        {
            if (_armorUI.gameObject.activeSelf)
            {
                _armorUI.ChangeSectionBack();
            }
        }
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_RB) == BUTTON_STATE.RELEASE)
        {
            if (_armorUI.gameObject.activeSelf)
            {
                _armorUI.ChangeSectionForward();
            }
        }
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_SELECT) == BUTTON_STATE.RELEASE)
        {
            return new UIInventoryState(UIManager.s.InventoryUI);
        }
        if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 >= 1)
        {
            _armorUI.ChangeItemUp();
        }
        if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 <= -1)
        {
            _armorUI.ChangeItemDown();
        }

        return this;
    }

    public override UIState OnUpdate()
    {
        return this;
    }
}
