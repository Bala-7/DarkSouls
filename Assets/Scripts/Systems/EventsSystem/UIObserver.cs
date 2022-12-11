using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIObserver : Observer
{
    public new delegate void OnNotifyDelegate();
    public new List<OnNotifyDelegate> OnNotify;
    public new List<UI_EVENTS> suscribedEvents;

    public UIObserver(List<UI_EVENTS> uiEvents, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = uiEvents;
        OnNotify = callbacks;
    }
}
