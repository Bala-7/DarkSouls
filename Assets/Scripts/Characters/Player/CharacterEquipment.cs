using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    private CharacterAttributes _attributes;

    [SerializeField] private int _headIndex = 7;
    [SerializeField] private int _torsoIndex = 19;
    [SerializeField] private int _armIndex = 10;
    [SerializeField] private int _legIndex = 10;
    [SerializeField] private int _swordIndex = 3;


    private List<Transform> _headItems;
    private List<Transform> _torsoItems;
    private List<Transform> _armUpperRightItems;
    private List<Transform> _armUpperLeftItems;
    private List<Transform> _armLowerRightItems;
    private List<Transform> _armLowerLeftItems;
    private List<Transform> _hipsItems;
    private List<Transform> _legRightItems;
    private List<Transform> _legLeftItems;
    [SerializeField] private Transform _swordsParent;
    private List<PlayerWeapon> _swordItems;


    private float _equipLoad = 0;
    public float EquipLoad { get { return _equipLoad; } }

    private void Awake()
    {
        _attributes = GetComponent<CharacterAttributes>();
        string genderString = ((int)(_attributes.Gender) == 0) ? "Male" : "Female";
        Transform partsRoot = transform.GetChild(0).Find(genderString + "_Parts");

        #region Get Items
        _headItems = new List<Transform>(partsRoot.Find(genderString + "_00_Head").GetChild(1).GetComponentsInChildren<Transform>());
        _headItems.RemoveAt(0);

        _torsoItems = new List<Transform>(partsRoot.Find(genderString + "_03_Torso").GetComponentsInChildren<Transform>());
        _torsoItems.RemoveAt(0);

        _armUpperRightItems = new List<Transform>(partsRoot.Find(genderString + "_04_Arm_Upper_Right").GetComponentsInChildren<Transform>());
        _armUpperRightItems.RemoveAt(0);
        _armUpperLeftItems = new List<Transform>(partsRoot.Find(genderString + "_05_Arm_Upper_Left").GetComponentsInChildren<Transform>());
        _armUpperLeftItems.RemoveAt(0);
        _armLowerRightItems = new List<Transform>(partsRoot.Find(genderString + "_06_Arm_Lower_Right").GetComponentsInChildren<Transform>());
        _armLowerRightItems.RemoveAt(0);
        _armLowerLeftItems = new List<Transform>(partsRoot.Find(genderString + "_07_Arm_Lower_Left").GetComponentsInChildren<Transform>());
        _armLowerLeftItems.RemoveAt(0);

        _hipsItems = new List<Transform>(partsRoot.Find(genderString + "_10_Hips").GetComponentsInChildren<Transform>());
        _hipsItems.RemoveAt(0);
        _legRightItems = new List<Transform>(partsRoot.Find(genderString + "_11_Leg_Right").GetComponentsInChildren<Transform>());
        _legRightItems.RemoveAt(0);
        _legLeftItems = new List<Transform>(partsRoot.Find(genderString + "_12_Leg_Left").GetComponentsInChildren<Transform>());
        _legLeftItems.RemoveAt(0);

        _swordItems = new List<PlayerWeapon>(_swordsParent.GetComponentsInChildren<PlayerWeapon>());
        #endregion

        #region Set Items
        SetNewHead(_headIndex);
        SetNewTorso(_torsoIndex);
        SetNewArms(_armIndex);
        SetNewLegs(_legIndex);
        SetNewSword(_swordIndex);
        CalculateEquipLoad();
        #endregion
    }

    private void CalculateEquipLoad()
    {
        _equipLoad = _headItems[_headIndex].GetComponent<ArmorPart>().Weight;
        _equipLoad += _torsoItems[_torsoIndex].GetComponent<ArmorPart>().Weight;

        _equipLoad += _armUpperRightItems[_armIndex].GetComponent<ArmorPart>().Weight;
        _equipLoad += _armUpperLeftItems[_armIndex].GetComponent<ArmorPart>().Weight;
        _equipLoad += _armLowerRightItems[_armIndex].GetComponent<ArmorPart>().Weight;
        _equipLoad += _armLowerLeftItems[_armIndex].GetComponent<ArmorPart>().Weight;

        _equipLoad += _hipsItems[_legIndex].GetComponent<ArmorPart>().Weight;
        _equipLoad += _legRightItems[_legIndex].GetComponent<ArmorPart>().Weight;
        _equipLoad += _legLeftItems[_legIndex].GetComponent<ArmorPart>().Weight;

        Debug.Log("Equipment Load is: " + _equipLoad);
    }

    #region Set Body Parts
    private void SetNewHead(int newIndex)
    {
        for (int i = 0; i < _headItems.Count; ++i)
        {
            bool enabled = (i == newIndex);
            _headItems[i].gameObject.SetActive(enabled);
        }
    }

    private void SetNewTorso(int newIndex)
    {
        for (int i = 0; i < _torsoItems.Count; ++i)
        {
            bool enabled = (i == newIndex);
            _torsoItems[i].gameObject.SetActive(enabled);
        }
    }

    private void SetNewArms(int newIndex)
    {
        for (int i = 0; i < _armUpperRightItems.Count; ++i)
        {
            bool enabled = (i == newIndex);
            _armUpperRightItems[i].gameObject.SetActive(enabled);
            _armUpperLeftItems[i].gameObject.SetActive(enabled);
            _armLowerRightItems[i].gameObject.SetActive(enabled);
            _armLowerLeftItems[i].gameObject.SetActive(enabled);
        }
    }

    private void SetNewLegs(int newIndex)
    {
        for (int i = 0; i < _hipsItems.Count; ++i)
        {
            bool enabled = (i == newIndex);
            _hipsItems[i].gameObject.SetActive(enabled);
            _legRightItems[i].gameObject.SetActive(enabled);
            _legLeftItems[i].gameObject.SetActive(enabled);
        }
    }

    private void SetNewSword(int newIndex)
    {
        for (int i = 0; i < _swordItems.Count; ++i)
        {
            bool enabled = (i == newIndex);
            _swordItems[i].gameObject.SetActive(enabled);
        }
    }

    public void ChangeHeadForward() 
    {
        int newIndex = (_headIndex + 1);
        _headIndex = (newIndex == _headItems.Count) ? 0 : newIndex;
        SetNewHead(_headIndex);
    }

    public void ChangeHeadBack()
    {
        int newIndex = (_headIndex -1);
        _headIndex = (newIndex == -1) ? _headItems.Count - 1 : newIndex;
        SetNewHead(_headIndex);
    }

    public void ChangeTorsoForward()
    {
        int newIndex = (_torsoIndex + 1);
        _torsoIndex = (newIndex == Mathf.Min(_torsoItems.Count, _armLowerLeftItems.Count)) ? 0 : newIndex;
        SetNewTorso(_torsoIndex);
        SetNewArms(_torsoIndex);
    }

    public void ChangeTorsoBack()
    {
        int newIndex = (_torsoIndex - 1);
        _torsoIndex = (newIndex == -1) ? Mathf.Min(_torsoItems.Count, _armLowerLeftItems.Count) - 1 : newIndex;
        SetNewTorso(_torsoIndex);
        SetNewArms(_torsoIndex);
    }

    public void ChangeLegsForward()
    {
        int newIndex = (_legIndex + 1);
        _legIndex = (newIndex == _legLeftItems.Count) ? 0 : newIndex;
        SetNewLegs(_legIndex);
    }

    public void ChangeLegsBack()
    {
        int newIndex = (_legIndex - 1);
        _legIndex = (newIndex == -1) ? _legLeftItems.Count - 1 : newIndex;
        SetNewLegs(_legIndex);
    }

    public void ChangeWeaponForward()
    {
        int newIndex = (_swordIndex + 1);
        _swordIndex = (newIndex == _swordItems.Count) ? 0 : newIndex;
        SetNewSword(_swordIndex);
    }

    public void ChangeWeaponBack()
    {
        int newIndex = (_swordIndex - 1);
        _swordIndex = (newIndex == -1) ? _swordItems.Count - 1 : newIndex;
        SetNewSword(_swordIndex);
    }

    public void ChangeShieldForward()
    {
        // TODO
    }

    public void ChangeShieldBack()
    {
        // TODO
    }

    #endregion

    #region Get Body Parts Info

    #region Head
    public string GetCurrentHeadName() { return _headItems[_headIndex].name; }

    public float GetCurrentHeadPhysicalProtection() { return _headItems[_headIndex].GetComponent<ArmorPart>().PhysicalProtection; }

    public float GetCurrentHeadMagicProtection() { return _headItems[_headIndex].GetComponent<ArmorPart>().MagicProtection; }

    public float GetCurrentHeadWeight() { return _headItems[_headIndex].GetComponent<ArmorPart>().Weight; }
    #endregion

    #region Torso
    public string GetCurrentTorsoName() { return _torsoItems[_torsoIndex].name; }

    public float GetCurrentTorsoPhysicalProtection() { return _torsoItems[_torsoIndex].GetComponent<ArmorPart>().PhysicalProtection; }

    public float GetCurrentTorsoMagicProtection() { return _torsoItems[_torsoIndex].GetComponent<ArmorPart>().MagicProtection; }

    public float GetCurrentTorsoWeight() { return _torsoItems[_torsoIndex].GetComponent<ArmorPart>().Weight; }

    #endregion

    #region Legs
    public string GetCurrentLegName() { return _legLeftItems[_legIndex].name; }

    public float GetCurrentLegPhysicalProtection() { return 2 * _legLeftItems[_legIndex].GetComponent<ArmorPart>().PhysicalProtection; }

    public float GetCurrentLegMagicProtection() { return 2 * _legLeftItems[_legIndex].GetComponent<ArmorPart>().MagicProtection; }

    public float GetCurrentLegWeight() { return 2 * _legLeftItems[_legIndex].GetComponent<ArmorPart>().Weight; }
    #endregion

    #region Sword

    public string GetCurrentWeaponName() { return _swordItems[_swordIndex].name; }

    public float GetCurrentWeaponPhysicalAttack() { return _swordItems[_swordIndex].GetComponent<PlayerWeapon>().PhysicalAttack; }

    public float GetCurrentWeaponMagicAttack() { return _swordItems[_swordIndex].GetComponent<PlayerWeapon>().MagicAttack; }

    public float GetCurrentWeaponWeight() { return _swordItems[_swordIndex].GetComponent<PlayerWeapon>().Weight; }
    #endregion

    #endregion
}
