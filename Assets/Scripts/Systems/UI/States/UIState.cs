using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIState
{
    public abstract UIState OnEnter();
    public abstract UIState OnUpdate();
    public abstract UIState OnExit();
    public abstract UIState OnInput(NewPlayerInput input);

}
