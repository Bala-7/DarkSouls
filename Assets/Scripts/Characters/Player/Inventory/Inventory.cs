using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [SerializeField] private List<InventoryObject> _inventoryObjects;
    [SerializeField] private List<InventoryObject> _bagObjects;
    private const int INVENTORY_OBJECTS_MAX_SIZE = 16;
    private const int BAG_OBJECTS_MAX_SIZE = 5;
    private int _currentBagObjectIndex = 0;

    private List<InventoryItemSO> _itemsData;

    private void Awake()
    {
        LoadItemData();

        _inventoryObjects = new List<InventoryObject>();
        _inventoryObjects.Add(new InventoryObject(this, "Small Potion"));
        _inventoryObjects.Add(new InventoryObject(this, "Potion"));
        _inventoryObjects.Add(new InventoryObject(this, "Big Potion"));
        for (int i = 3; i < INVENTORY_OBJECTS_MAX_SIZE; ++i)
        {
            _inventoryObjects.Add(new InventoryObject(this, "empty"));
        }

        _bagObjects = new List<InventoryObject>();
        for (int i = 0; i < BAG_OBJECTS_MAX_SIZE; ++i)
        {
            _bagObjects.Add(new InventoryObject(this, "empty"));
        }
    }

    public InventoryItemSO GetItemSO(string scriptableObjectName)
    {
        foreach (InventoryItemSO so in _itemsData)
        {
            if (so.name.Equals(scriptableObjectName))
                return so;
        }
        return null;
    }

    private void LoadItemData()
    {
        _itemsData = new List<InventoryItemSO>(Resources.LoadAll<InventoryItemSO>("ScriptableObjects"));
    }

    public List<InventoryObject> GetInventoryItems()
    {
        return _inventoryObjects;
    }

    public List<InventoryObject> GetBagItems()
    {
        return _bagObjects;
    }

    public void PickDrop(List<InventoryObject> newObjects)
    {
        foreach (InventoryObject obj in newObjects)
        {
            bool inObjects = _inventoryObjects.Contains(obj);
            bool inUsable = _bagObjects.Contains(obj);
            int indexToInsert = GetFirstEmptyInventoryIndex();
            bool inventoryFull = (indexToInsert == -1);

            if (!inObjects && !inUsable && !inventoryFull) 
            { 
                _inventoryObjects[indexToInsert] = obj;
            }
            else if (inObjects)
            {
                IncreaseObjectInList(obj, _inventoryObjects);
            }
            else if (inUsable)
            {
                IncreaseObjectInList(obj, _bagObjects);
            }
        }
    }

    private int GetFirstEmptyInventoryIndex()
    {
        for (int i = 0; i < INVENTORY_OBJECTS_MAX_SIZE; ++i)
        {
            if (_inventoryObjects[i].GetName().Equals("empty"))
                return i;
        }
        return -1;
    }

    private void IncreaseObjectInList(InventoryObject obj, List<InventoryObject> list)
    {
        int index = list.IndexOf(obj);
        list[index].IncreaseAmount(obj.GetAmount());
    }

    public void MoveObjectToBag(int inventoryIndex, int bagIndex)
    {
        bool bagEmpty = IsBagEmptyAt(bagIndex);
        bool inventoryEmpty = IsInventoryEmptyAt(inventoryIndex);

        InventoryObject bagItem = new InventoryObject(this, "empty");
        InventoryObject inventoryItem = new InventoryObject(this, "empty");

        if (!bagEmpty)
            bagItem = _bagObjects[bagIndex];
        if(!inventoryEmpty)
            inventoryItem = _inventoryObjects[inventoryIndex];

        _inventoryObjects[inventoryIndex] = bagItem;
        _bagObjects[bagIndex] = inventoryItem;
    }

    public bool IsInventoryEmptyAt(int index)
    { 
        return ReferenceEquals(_inventoryObjects[index], null) || _inventoryObjects[index].GetName().Equals("empty"); 
    }

    public bool IsBagEmptyAt(int index)
    {
        return ReferenceEquals(_bagObjects[index], null) || _bagObjects[index].GetName().Equals("empty");
    }

    public bool IsBagEmpty()
    {
        for (int i = 0; i < _bagObjects.Count; ++i)
            if (!IsBagEmptyAt(i))
                return false;
        return true;
    }

    public Sprite GetCurrentBagObjectIcon()
    {
        return _bagObjects[_currentBagObjectIndex].GetIcon();
    }

    public void UseCurrentItem()
    {
        _bagObjects[_currentBagObjectIndex].UseItem();
        Debug.LogWarning(_bagObjects[_currentBagObjectIndex].GetName() + " was used!");
    }

    public void NextCurrentObject() 
    {
        if (!IsBagEmpty()) 
        {
            do
            {
                _currentBagObjectIndex = (_currentBagObjectIndex + 1) % BAG_OBJECTS_MAX_SIZE;
            }
            while (IsBagEmptyAt(_currentBagObjectIndex));
            EventsManager.instance.NotifyEvent(UI_EVENTS.PLAYER_EQUIPPED_ITEM_CHANGE);
        }
    }
}
