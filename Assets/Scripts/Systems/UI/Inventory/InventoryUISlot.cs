using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour
{
    private Image _icon;
    private TMP_Text _nameText;


    private void Awake()
    {
        try
        {
            _icon = transform.Find("Icon").GetComponent<Image>();
            _nameText = transform.Find("NameText").GetComponent<TMP_Text>();
        }
        catch (Exception e)
        {
            Debug.LogWarning("A component in InventoryUISlot object " + name + " is missing!");
        }
    }

    public void LoadItemData(InventoryObject item)
    {
        _nameText.text = item.GetName();
        _icon.sprite = item.GetIcon();
    }

    public void EnableSlot()
    {
        _icon.gameObject.SetActive(true);
        _nameText?.gameObject.SetActive(true);
    }

    public void DisableSlot()
    {
        _icon.gameObject.SetActive(false);
        _nameText?.gameObject.SetActive(false);
    }
}
