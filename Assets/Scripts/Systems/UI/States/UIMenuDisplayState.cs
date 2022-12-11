using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMenuDisplayState : UIState
{


    private UIState _currentSubstate;

    private InventoryUI _inventoryUI;
    private ArmorUI _armorUI;
    public UIMenuDisplayState(InventoryUI inventoryUI, ArmorUI armorUI)
    {
        _inventoryUI = inventoryUI;
        _armorUI = armorUI;
    }

    public override UIState OnEnter()
    {
        _armorUI.gameObject.SetActive(true);
        _armorUI.EnableArmorUI();

        _inventoryUI.gameObject.SetActive(true);
        _inventoryUI.RefreshInventoryUI();
        
        ThirdPersonControllerMovement.s.DisableMovement();

        _currentSubstate = new UIArmorState(_armorUI);
        _currentSubstate.OnEnter();
        return this;
    }

    public override UIState OnExit()
    {
        _armorUI.gameObject.SetActive(false);
        _inventoryUI.gameObject.SetActive(false);
        ThirdPersonControllerMovement.s.EnableMovement();
        return this;
    }

    public override UIState OnUpdate()
    {
        return this;
    }

    public override UIState OnInput(NewPlayerInput input)
    {
        if (input.GetButtonState(INPUT_BUTTONS.BUTTON_START) == BUTTON_STATE.RELEASE)
        {
            return new UIGameplayState();
        }

        UIState prevState = _currentSubstate;
        _currentSubstate = _currentSubstate.OnInput(input);
        if (prevState != _currentSubstate)
        {
            prevState.OnExit();
            _currentSubstate.OnEnter();
        }
        return this;
    }
}
