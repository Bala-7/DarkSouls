using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerformance : CharacterPerformance
{
    
    private float _stamina;
    private float _equipLoad;
    private int _rightHandDamage;
    private int _physicalDefense;
    private int _magicDefense;
    private int _itemDiscovery;
    private int _souls;
    public int Souls { get { return _souls; } }

    private CharacterAttributes attributes;

    [SerializeField] private AnimationCurve _rollingMovementBase;

    private const float _staminaAttackA = 15f;
    private const float _staminaRoll = 7.5f;

    private EnemyObserver _enemyObserver;

    private void Awake()
    {
        attributes = GetComponent<CharacterAttributes>();
        _hp = attributes.VitalityMax;
        _stamina = attributes.StaminaMax;
        _souls = 0;

        _enemyObserver = new EnemyObserver(new List<ENEMY_EVENTS>() { ENEMY_EVENTS.ENEMY_DEAD },
            new List<EnemyObserver.OnNotifyDelegate>() { OnEnemyDefeated });
    }

    public void Restart()
    {
        _hp = attributes.VitalityMax;
        _stamina = attributes.StaminaMax;
        _souls = 0;

        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_HEALTH_CHANGE, _hp, attributes.VitalityMax, attributes.VitalityMax);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_STAMINA_CHANGE, _stamina, attributes.StaminaMax, attributes.StaminaMax);
    }

    private void Start()
    {
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_HEALTH_CHANGE, _hp, attributes.VitalityMax, attributes.VitalityMax);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_STAMINA_CHANGE, _stamina, attributes.StaminaMax, attributes.StaminaMax);
    }

    public void PlayerRunningTick()
    {
        float previousValue = _stamina;
        _stamina = Mathf.Max(_stamina - .02f, 0f);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_STAMINA_CHANGE, _stamina, previousValue, attributes.StaminaMax);
    }

    public void PlayerNotRunningTick()
    {
        float previousValue = _stamina;
        _stamina = Mathf.Min(_stamina + .25f, attributes.StaminaMax);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_STAMINA_CHANGE, _stamina, previousValue, attributes.StaminaMax);
    }

    public float EvaluateRollCurve(float t)
    {
        return _rollingMovementBase.Evaluate(t);
    }

    public bool HasEnoughStaminaForRoll()
    {
        return _stamina >= 0.2f * _staminaRoll;
    }

    public void Roll()
    {
        float previousValue = _stamina;
        _stamina = Mathf.Max(_stamina - _staminaRoll, 0f);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_STAMINA_CHANGE, _stamina, previousValue, attributes.StaminaMax);
    }

    public void PlayerAttackA()
    {
        float previousValue = _stamina;
        _stamina = Mathf.Max(_stamina - _staminaAttackA, 0f);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_STAMINA_CHANGE, _stamina, previousValue, attributes.StaminaMax);
    }

    public bool HasEnoughStaminaForAttackA()
    {
        return _stamina >= 0.66f * _staminaAttackA;
    }

    public bool HasEnoughStaminaForRun()
    {
        return _stamina > 0;
    }

    public void RestoreHealth(float hpToRestore)
    {
        float previousValue = _hp;
        _hp = Mathf.Min(attributes.VitalityMax, _hp + hpToRestore);
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_HEALTH_CHANGE, _hp, previousValue, attributes.VitalityMax);
    }


    public void AddSouls(int amount)
    {
        _souls += amount;
    }

    #region Inherited Methods
    public override bool DealDamage()
    {
        _hp = Mathf.Max(_hp - 10, 0);
        return (_hp > 0);
    }
    #endregion

    private void OnEnemyDefeated(Enemy e)
    {
        _souls += e.SoulsDrop;
    }
}
