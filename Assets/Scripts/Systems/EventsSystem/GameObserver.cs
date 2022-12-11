using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObserver : Observer
{
    public new delegate void OnNotifyDelegate();
    public new List<OnNotifyDelegate> OnNotify;
    public new List<GAME_EVENTS> suscribedEvents;

    public GameObserver(List<GAME_EVENTS> gameEvents, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = gameEvents;
        OnNotify = callbacks;
    }
}
