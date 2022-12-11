using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryObject : System.IEquatable<InventoryObject>
{
    private int _amount;
    private string _name;

    private InventoryItemSO _data;
    private Inventory _inventory;

    public InventoryObject(Inventory inventory, string scriptableObjectName)
    {
        _inventory = inventory;
        _data = inventory.GetItemSO(scriptableObjectName);
        _name = _data.name;
        _amount = 1;
    }

    public bool Equals(InventoryObject other)
    {
        if (ReferenceEquals(other, null)) return false;
        return (_name.Equals(other._name));
    }

    public string GetName()
    {
        return _name;
    }

    public int GetAmount()
    {
        return _amount;
    }

    public Sprite GetIcon()
    {
        return _data.icon;
    }

    public void IncreaseAmount(int amountToAdd)
    {
        _amount += amountToAdd;
    }

    public void DecreaseAmount(int amountToSubstract)
    {
        _amount += amountToSubstract;
    }

    public void UseItem()
    {
        switch (_name)
        {
            case ("Small Potion"): UseSmallPotion(); break;
            case ("Potion"): UsePotion(); break;
            case ("Big Potion"): UseBigPotion(); break;
            default: Debug.LogError("Trying to use an unknown object. Please check this!"); break;
        }
    }

    #region Item Use Implementations
    private void UseSmallPotion()
    {
        ThirdPersonControllerMovement.s.Performance.RestoreHealth(20);
        Debug.Log("Restoring 20HP.");
    }
    private void UsePotion()
    {
        ThirdPersonControllerMovement.s.Performance.RestoreHealth(50);
        Debug.Log("Restoring 50HP.");
    }
    private void UseBigPotion()
    {
        ThirdPersonControllerMovement.s.Performance.RestoreHealth(100);
        Debug.Log("Restoring 100HP.");
    }
    #endregion
}
