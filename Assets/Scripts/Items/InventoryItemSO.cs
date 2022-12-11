using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Create Inventory Item")]
public class InventoryItemSO : ScriptableObject, System.IEquatable<InventoryItemSO>
{
    public new string name;
    public string description;

    public Sprite icon;

    public bool Equals(InventoryItemSO other)
    {
        return name.Equals(other.name);
    }
}
