using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryState : UIState
{
    private InventoryUI _inventoryUI;
    public UIInventoryState(InventoryUI inventoryUI)
    {
        _inventoryUI = inventoryUI;
    }



    public override UIState OnEnter()
    {
        _inventoryUI.OnInventoryUISelected();
        return this;
    }

    public override UIState OnExit()
    {
        _inventoryUI.OnInventoryUIUnselected();
        return this;
    }

    public override UIState OnInput(NewPlayerInput input)
    {
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_A) == BUTTON_STATE.RELEASE)
        {
            if (_inventoryUI.IsInEquippedSelection)
                _inventoryUI.OnBagSlotSelected();
            else
                _inventoryUI.OnInventorySlotSelected();
        }
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_SELECT) == BUTTON_STATE.RELEASE)
        {
            return new UIArmorState(UIManager.s.ArmorUI);
        }

        if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 <= -1)
        {
            _inventoryUI.MoveSelectionBox(0, -1);
        }
        if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 >= 1)
        {
            _inventoryUI.MoveSelectionBox(0, 1);
        }
        if (input.AxisStates[(int)INPUT_AXIS.DPH].Item2 <= -1)
        {
            _inventoryUI.MoveSelectionBox(1, 0);
        }
        if (input.AxisStates[(int)INPUT_AXIS.DPH].Item2 >= 1)
        {
            _inventoryUI.MoveSelectionBox(-1, 0);
        }
        if (Mathf.Abs(input.AxisStates[(int)INPUT_AXIS.DPV].Item2) <= 0.2f &&
            Mathf.Abs(input.AxisStates[(int)INPUT_AXIS.DPH].Item2) <= 0.2f)
        {
            _inventoryUI.ButtonReleased();
        }
        return this;
    }
    public override UIState OnUpdate()
    {
        return this;
    }
}
