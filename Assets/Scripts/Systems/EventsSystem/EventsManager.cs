using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EventsManager : MonoBehaviour
{
    public static EventsManager instance;
    private List<Observer> _observers;

    private void Awake()
    {
        if(!instance)
            instance = this;

        _observers = new List<Observer>();
    }

    public void RegisterObserver(Observer observer)
    {
        _observers.Add(observer);
    }

    public void NotifyEvent(EVENTS newEvent)
    {
        foreach (Observer obs in _observers)
        {
            if(obs.suscribedEvents.Contains(newEvent))
                obs.OnNotify[obs.suscribedEvents.IndexOf(newEvent)]();
        }
    }

    public void NotifyEvent(GAME_EVENTS newEvent)
    {
        foreach (Observer obs in _observers)
        {
            if (obs is GameObserver)
            {
                GameObserver go = (GameObserver)obs;
                if (go.suscribedEvents.Contains(newEvent))
                    go.OnNotify[go.suscribedEvents.IndexOf(newEvent)]();
            }
        }
    }

    public void NotifyEvent(PLAYER_EVENTS newEvent, float newValue, float currentValue, float maxValue)
    {
        foreach (Observer obs in _observers)
        {
            if (obs is PlayerObserver)
            {
                PlayerObserver po = (PlayerObserver)obs;
                if (po.suscribedEvents.Contains(newEvent))
                    po.OnNotify[po.suscribedEvents.IndexOf(newEvent)](newValue, currentValue, maxValue);
            }
        }
    }

    #region Player Input Events

    /*public void NotifyEvent(PLAYER_INPUT newEvent, PlayerInput? input)
    {
        foreach (Observer obs in _observers)
        {
            if (obs is PlayerInputObserver)
            {
                PlayerInputObserver po = (PlayerInputObserver)obs;
                if (po.suscribedEvents.Contains(newEvent))
                    po.OnNotify[po.suscribedEvents.IndexOf(newEvent)](input);
            }
        }
    }*/

    public void NotifyEvent(PLAYER_INPUT newEvent, NewPlayerInput input)
    {
        try
        {
            foreach (Observer obs in _observers)
            {
                if (obs is PlayerInputObserver)
                {
                    PlayerInputObserver po = (PlayerInputObserver)obs;
                    if (po.suscribedEvents.Contains(newEvent))
                        po.OnNotify[po.suscribedEvents.IndexOf(newEvent)](input);
                }
            }
        }
        catch (InvalidOperationException e)
        {
            Debug.LogWarning("Failed to notify event " + newEvent.ToString());
        }
    }

    #endregion

    public void NotifyEvent(ENEMY_EVENTS newEvent, Enemy enemy)
    {
        foreach (Observer obs in _observers)
        {
            if (obs is EnemyObserver)
            {
                EnemyObserver po = (EnemyObserver)obs;
                if (po.suscribedEvents.Contains(newEvent))
                    po.OnNotify[po.suscribedEvents.IndexOf(newEvent)](enemy);
            }
        }
    }

    public void NotifyEvent(BOSS_EVENTS newEvent, Enemy enemy)
    {
        foreach (Observer obs in _observers)
        {
            if (obs is BossObserver)
            {
                BossObserver po = (BossObserver)obs;
                if (po.suscribedEvents.Contains(newEvent))
                    po.OnNotify[po.suscribedEvents.IndexOf(newEvent)](enemy);
            }
        }
    }

    public void NotifyEvent(UI_EVENTS newEvent)
    {
        foreach (Observer obs in _observers)
        {
            if (obs is UIObserver)
            {
                UIObserver po = (UIObserver)obs;
                if (po.suscribedEvents.Contains(newEvent))
                    po.OnNotify[po.suscribedEvents.IndexOf(newEvent)]();
            }
        }
    }
}
