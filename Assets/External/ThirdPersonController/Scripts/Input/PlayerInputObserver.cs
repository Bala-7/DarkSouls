using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputObserver : Observer
{
    public new delegate void OnNotifyDelegate(NewPlayerInput input);
    public new List<OnNotifyDelegate> OnNotify;
    public new List<PLAYER_INPUT> suscribedEvents;

    public PlayerInputObserver(List<PLAYER_INPUT> playerEvents, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = playerEvents;
        OnNotify = callbacks;
    }
}
