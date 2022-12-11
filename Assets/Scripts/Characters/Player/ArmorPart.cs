using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorPart : MonoBehaviour
{
    [SerializeField] private float _physicalProtection;
    [SerializeField] private float _magicProtection;
    [SerializeField] private float _weight;

    public enum ARMOR_TYPE { HELMET = 0, ARMOR, ARMS, LEGS }
    public ARMOR_TYPE type;

    private void Awake()
    {
        _physicalProtection = GenerateRandomPhysicalDefense();
        _magicProtection = GenerateRandomMagicDefense();
        _weight = GenerateRandomWeight();
    }

    private float GenerateRandomPhysicalDefense()
    {
        float result = 0;
        switch (type)
        {
            case ARMOR_TYPE.HELMET: result = Random.Range(0.5f, 5.5f); break;
            case ARMOR_TYPE.ARMOR: result = Random.Range(2.5f, 20.5f); break;
            case ARMOR_TYPE.ARMS: result = Random.Range(0.5f, 5.5f); break;
            case ARMOR_TYPE.LEGS: result = Random.Range(0.5f, 5.5f); break;
            default: break;
        }
        return result;
    }

    private float GenerateRandomMagicDefense()
    {
        float result = 0;
        switch (type)
        {
            case ARMOR_TYPE.HELMET: result = Random.Range(0.5f, 3.5f); break;
            case ARMOR_TYPE.ARMOR: result = Random.Range(1.5f, 8.5f); break;
            case ARMOR_TYPE.ARMS: result = Random.Range(0.5f, 5.5f); break;
            case ARMOR_TYPE.LEGS: result = Random.Range(0.5f, 3.5f); break;
            default: break;
        }
        return result;
    }

    private float GenerateRandomWeight()
    {
        float result = 0;
        switch (type)
        {
            case ARMOR_TYPE.HELMET: result = _physicalProtection * 1.5f; break;
            case ARMOR_TYPE.ARMOR: result = _physicalProtection * 1.5f; break;
            case ARMOR_TYPE.ARMS: result = (_physicalProtection * 1.5f) / 4; break;
            case ARMOR_TYPE.LEGS: result = (_physicalProtection * 1.5f) / 2; break;
            default: break;
        }
        return result;
    }

    public float PhysicalProtection { get { return _physicalProtection; } }
    public float MagicProtection { get { return _magicProtection; } }
    public float Weight { get { return _weight; } }
    
}
