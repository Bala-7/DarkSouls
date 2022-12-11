using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObserver : Observer
{
    public new delegate void OnNotifyDelegate(float newValue, float currentValue, float maxValue);
    public new List<OnNotifyDelegate> OnNotify;
    public new List<PLAYER_EVENTS> suscribedEvents;

    public PlayerObserver(List<PLAYER_EVENTS> playerEvents, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = playerEvents;
        OnNotify = callbacks;
    }
}
