using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer
{
    public delegate void OnNotifyDelegate();
    public List<OnNotifyDelegate> OnNotify;
    public List<EVENTS> suscribedEvents;

    public Observer() 
    {
    
    }

    public Observer(List<EVENTS> events, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = events;
        OnNotify = callbacks;
    }
}
