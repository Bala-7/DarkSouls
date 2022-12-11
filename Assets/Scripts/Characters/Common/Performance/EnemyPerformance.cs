using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPerformance : CharacterPerformance
{
    protected int _physicalDamage;
    protected int _magicDamage;
    protected int _physicalDefense;
    protected float _maxHp = 36f;

    #region Inherited Methods
    public override bool DealDamage()
    {
        _hp = Mathf.Max(_hp - 7, 0);
        return (_hp > 0);
    }
    #endregion

    public bool IsAliveAfterNextHit()
    {
        return _hp > 7;
    }

    private void Awake()
    {
        _maxHp = 36f;
        _physicalDamage = 10;
        _magicDamage = 0;
        _physicalDefense = 50;
        _hp = _maxHp;
    }

    public float GetCurrentHealthPercentage() { return _hp / _maxHp; }

    public bool IsMaxHp() { return _hp == _maxHp; }

    public bool NextHitWillKill() { return _hp - 7 > 0; }

    public void Restart()
    {
        _hp = _maxHp;
    }
}
