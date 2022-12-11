using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObserver : Observer
{
    public new delegate void OnNotifyDelegate(Enemy enemy);
    public new List<OnNotifyDelegate> OnNotify;
    public new List<BOSS_EVENTS> suscribedEvents;

    public BossObserver(List<BOSS_EVENTS> bossEvents, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = bossEvents;
        OnNotify = callbacks;
    }
}
