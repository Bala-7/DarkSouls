using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmorUI : MonoBehaviour
{
    private Image _mainIcon;
    private Image _itemIconA;
    private Image _itemIconB;

    private TMP_Text _nameText;
    private TMP_Text _itemAText;
    private TMP_Text _itemBText;
    private TMP_Text _weightText;

    public List<Sprite> sectionIcons;
    public List<Sprite> weaponIcons;
    public List<Sprite> armorIcons;


    private enum SHOW_ARMOR_STATE { WEAPON = 0, SHIELD, HELMET, ARMOR, BOOTS }
    private SHOW_ARMOR_STATE currentState;

    private CharacterEquipment _equipment;

    private GameObject _frameArmorUISelected;

    private List<Action> changeItemUpMethods;
    private List<Action> changeItemDownMethods;


    private void Awake()
    {
        _mainIcon = transform.Find("MainIcon").GetComponent<Image>();
        _itemIconA = transform.Find("ItemIconA").GetComponent<Image>();
        _itemIconB = transform.Find("ItemIconB").GetComponent<Image>();

        _nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        _itemAText = transform.Find("ItemTextA").GetComponent<TMP_Text>();
        _itemBText = transform.Find("ItemTextB").GetComponent<TMP_Text>();
        _weightText = transform.Find("WeightText").GetComponent<TMP_Text>();

        _frameArmorUISelected = transform.Find("SelectionFrameArmorUI").gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.SetActive(false);
        _equipment = ThirdPersonControllerMovement.s.Equipment;

        changeItemUpMethods = new List<Action> { _equipment.ChangeWeaponBack, _equipment.ChangeShieldBack, _equipment.ChangeHeadBack, _equipment.ChangeTorsoBack, _equipment.ChangeLegsBack };
        changeItemDownMethods = new List<Action> { _equipment.ChangeWeaponForward, _equipment.ChangeShieldForward, _equipment.ChangeHeadForward, _equipment.ChangeTorsoForward, _equipment.ChangeLegsForward };
    }

    public void OnArmorUISelected()
    {
        _frameArmorUISelected.SetActive(true);
    }

    public void OnArmorUIUnselected()
    {
        _frameArmorUISelected.SetActive(false);
    }

    public void EnableArmorUI()
    {
        if(!_equipment) _equipment = ThirdPersonControllerMovement.s.Equipment;
        currentState = SHOW_ARMOR_STATE.WEAPON;
        _mainIcon.sprite = sectionIcons[(int)currentState];
        UpdateSectionTextsAndIcons();
    }

    public void ChangeSectionBack() 
    {
        int nextIndex = ((int)currentState - 1);
        nextIndex = (nextIndex >= 0) ? nextIndex : sectionIcons.Count + nextIndex; 
        currentState = (SHOW_ARMOR_STATE)(nextIndex);
        _mainIcon.sprite = sectionIcons[(int)currentState];
        UpdateSectionTextsAndIcons();

    }

    public void ChangeSectionForward()
    {
        currentState = (SHOW_ARMOR_STATE)(((int)currentState + 1) % sectionIcons.Count);
        
        UpdateSectionTextsAndIcons();
    }

    public void ChangeItemUp()
    {
        changeItemUpMethods[(int)currentState]();
        UpdateSectionTextsAndIcons();
    }

    public void ChangeItemDown()
    {
        changeItemDownMethods[(int)currentState]();
        UpdateSectionTextsAndIcons();
    }

    private void UpdateSectionTextsAndIcons()
    {
        _mainIcon.sprite = sectionIcons[(int)currentState];
        _itemIconA.sprite = GetCurrentItemAIcon();
        _itemIconB.sprite = GetCurrentItemBIcon();

        _nameText.text = GetCurrentNameText();
        _itemAText.text = GetCurrentItemAValue().ToString("0.##");
        _itemBText.text = GetCurrentItemBValue().ToString("0.##");
        _weightText.text = GetCurrentWeight().ToString("0.##");
    }

    private Sprite GetCurrentItemAIcon()
    {
        return (currentState == SHOW_ARMOR_STATE.WEAPON) ? weaponIcons[0] : armorIcons[0];
    }

    private Sprite GetCurrentItemBIcon()
    {
        return (currentState == SHOW_ARMOR_STATE.WEAPON) ? weaponIcons[1] : armorIcons[1];
    }

    private string GetCurrentNameText() 
    {
        string result = "";
        switch (currentState)
        {
            case SHOW_ARMOR_STATE.HELMET: result = _equipment.GetCurrentHeadName(); break;
            case SHOW_ARMOR_STATE.ARMOR: result = _equipment.GetCurrentTorsoName(); break;
            case SHOW_ARMOR_STATE.BOOTS: result = _equipment.GetCurrentLegName(); break;
            case SHOW_ARMOR_STATE.WEAPON: result = _equipment.GetCurrentWeaponName(); break;
            case SHOW_ARMOR_STATE.SHIELD: result = "Default Shield"; break;
            default: result = "Invalid Section"; break;
        }
        return result;
    }

    private float GetCurrentItemAValue()
    {
        float result = 0;
        switch (currentState)
        {
            case SHOW_ARMOR_STATE.HELMET: result = _equipment.GetCurrentHeadPhysicalProtection(); break;
            case SHOW_ARMOR_STATE.ARMOR: result = _equipment.GetCurrentTorsoPhysicalProtection(); break;
            case SHOW_ARMOR_STATE.BOOTS: result = _equipment.GetCurrentLegPhysicalProtection(); break;
            case SHOW_ARMOR_STATE.WEAPON: result = _equipment.GetCurrentWeaponPhysicalAttack(); break;
            case SHOW_ARMOR_STATE.SHIELD: result = -1; break;
            default: result = -1; break;
        }
        return result;
    }

    private float GetCurrentItemBValue()
    {
        float result = 0;
        switch (currentState)
        {
            case SHOW_ARMOR_STATE.HELMET: result = _equipment.GetCurrentHeadMagicProtection(); break;
            case SHOW_ARMOR_STATE.ARMOR: result = _equipment.GetCurrentTorsoMagicProtection(); break;
            case SHOW_ARMOR_STATE.BOOTS: result = _equipment.GetCurrentLegMagicProtection(); break;
            case SHOW_ARMOR_STATE.WEAPON: result = _equipment.GetCurrentWeaponMagicAttack(); break;
            case SHOW_ARMOR_STATE.SHIELD: result = -1; break;
            default: result = -1; break;
        }
        return result;
    }

    private float GetCurrentWeight()
    {
        float result = 0;
        switch (currentState)
        {
            case SHOW_ARMOR_STATE.HELMET: result = _equipment.GetCurrentHeadWeight(); break;
            case SHOW_ARMOR_STATE.ARMOR: result = _equipment.GetCurrentTorsoWeight(); break;
            case SHOW_ARMOR_STATE.BOOTS: result = _equipment.GetCurrentLegWeight(); break;
            case SHOW_ARMOR_STATE.WEAPON: result = _equipment.GetCurrentWeaponWeight(); break;
            case SHOW_ARMOR_STATE.SHIELD: result = -1; break;
            default: result = -1; break;
        }
        return result;
    }

}
