using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public enum GENDER { MALE = 0, FEMALE }
    [SerializeField] private GENDER _gender;
    public GENDER Gender { get { return _gender; } }

    [SerializeField] private int _vitality;
    public int VitalityMax { get { return _vitality; } }
    [SerializeField] private int _stamina;
    public int StaminaMax { get { return _stamina; }  }

    [SerializeField] private int _strength;
    [SerializeField] private int _dexterity;

    [SerializeField] private float _equipLoadMax;
    public float EquipLoadMax { get { return _equipLoadMax; } }
    

}
