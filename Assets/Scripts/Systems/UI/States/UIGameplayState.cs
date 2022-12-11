using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGameplayState : UIState
{
    private UIManager _uiManager;

    public UIGameplayState()
    {
        _uiManager = UIManager.s;
    }

    public override UIState OnEnter()
    {
        _uiManager.HideAllMenus();
        return this;
    }

    public override UIState OnExit()
    {
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
            return new UIMenuDisplayState(_uiManager.InventoryUI, _uiManager.ArmorUI);
        }
        if ((input.GetButtonState(INPUT_BUTTONS.BUTTON_A) == BUTTON_STATE.RELEASE) && UIManager.s.InfoMessage.IsShowing())
        {
            UIManager.s.InfoMessage.SetShowingMessage(false);
        }

        return this;
    }
}
