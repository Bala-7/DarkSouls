using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObserver : Observer
{
    public new delegate void OnNotifyDelegate(Enemy enemy);
    public new List<OnNotifyDelegate> OnNotify;
    public new List<ENEMY_EVENTS> suscribedEvents;

    public EnemyObserver(List<ENEMY_EVENTS> enemyEvents, List<OnNotifyDelegate> callbacks)
    {
        suscribedEvents = enemyEvents;
        OnNotify = callbacks;
    }
}
