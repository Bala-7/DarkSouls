using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterPerformance : MonoBehaviour
{
    protected float _hp;
    public float HP { get { return _hp; } }

    // Returns true if the character is still alive after the damage
    public abstract bool DealDamage();
}
