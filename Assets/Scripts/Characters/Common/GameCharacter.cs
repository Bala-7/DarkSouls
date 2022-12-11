using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{
    [SerializeField]
    protected Transform weaponsParent;
    protected bool alive = true;

    protected virtual void Awake()
    {
        InitializePerformance();
    }

    protected virtual void Start()
    { 
    
    }

    public virtual void OnReceiveHit() { }

    protected abstract void InitializePerformance();

    public abstract void EnableCurrentWeapons();

    public abstract void DisableCurrentWeapons();

    public bool IsAlive() { return alive; }

    public abstract void OnCharacterSpawn();
}
