using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private GameObject _frameInventoryUISelected;
    private GameObject _frameBagSelected;
    private GameObject _frameInventorySelected;

    private const int BAG_ROWS = 2;
    private const int BAG_COLUMNS = 8;

    private const int EQUIPPED_ITEMS = 5;

    private int _currentSelectedInventoryItem = 0;
    private int _currentSelectedBagItem = 0;

    private Vector3 _startPositionInventoryFrame;
    private Vector3 _startPositionBagFrame;

    private bool inEquippedSelection = true;
    public bool IsInEquippedSelection { get { return inEquippedSelection; } }

    bool needsRelease = false;

    private Inventory _inventory;

    private List<InventoryUISlot> _inventorySlots;
    private List<InventoryUISlot> _bagSlots;

    private Image _inGameEquippedItemIcon;
    private Image _inGameEquippedWeapon;
    private Image _inGameEquippedShield;
    private Image _inGameEquippedMagic;

    #region Observer
    private UIObserver _observer;
    #endregion

    private void Awake()
    {
        _frameInventoryUISelected = transform.Find("SelectionFrameInventoryUI").gameObject;
        
        _frameBagSelected = transform.Find("SelectionFrameEquipped").gameObject;
        _startPositionBagFrame = _frameBagSelected.transform.position;

        _frameInventorySelected = transform.Find("SelectionFrameInventory").gameObject;
        _startPositionInventoryFrame = _frameInventorySelected.transform.position;

        _inventorySlots = new List<InventoryUISlot>(transform.Find("InventoryItems").GetComponentsInChildren<InventoryUISlot>());
        _bagSlots = new List<InventoryUISlot>(transform.Find("BagSlots").GetComponentsInChildren<InventoryUISlot>());

        _inGameEquippedItemIcon = transform.parent.Find("WeaponsUI/SlotDown/Icon").GetComponent<Image>();
        _inGameEquippedWeapon = transform.parent.Find("WeaponsUI/SlotLeft/Icon").GetComponent<Image>();
        _inGameEquippedShield = transform.parent.Find("WeaponsUI/SlotRight/Icon").GetComponent<Image>();
        _inGameEquippedMagic = transform.parent.Find("WeaponsUI/SlotUp/Icon").GetComponent<Image>();

        //RefreshInventoryUI();
        //_inGameEquippedItemIcon.gameObject.SetActive(false);
        //_inGameEquippedWeapon.gameObject.SetActive(false);
        //_inGameEquippedShield.gameObject.SetActive(false);
        //_inGameEquippedMagic.gameObject.SetActive(false);

        _inventory = ThirdPersonControllerMovement.s.GetComponentInChildren<Inventory>();

        _observer = new UIObserver(new List<UI_EVENTS> 
        {
            UI_EVENTS.PLAYER_EQUIPPED_ITEM_CHANGE
        },
        new List<UIObserver.OnNotifyDelegate> 
        {
            OnItemChange
        });
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
        EventsManager.instance.RegisterObserver(_observer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnInventoryUISelected()
    {
        ResetSelectionFrames();

        _frameInventoryUISelected.SetActive(true);
        _frameInventorySelected.SetActive(false);
        _frameBagSelected.SetActive(true);

        inEquippedSelection = true;
    }

    public void OnInventoryUIUnselected()
    {
        _frameInventoryUISelected.SetActive(false);
    }



    public void MoveSelectionBox(int xAmount, int yAmount)
    {
        if (!needsRelease) 
        {
            if (inEquippedSelection)
            {
                if (Sign(xAmount) > 0 && _currentSelectedBagItem < EQUIPPED_ITEMS - 1)
                {
                    _frameBagSelected.transform.position += 160 * Vector3.right;
                    _currentSelectedBagItem += (int)Sign(xAmount);
                }
                else if (Sign(xAmount) < 0 && _currentSelectedBagItem > 0)
                {
                    _frameBagSelected.transform.position -= 160 * Vector3.right;
                    _currentSelectedBagItem += (int)Sign(xAmount);
                }
            }
            else
            {
                if (Sign(xAmount) > 0 && _currentSelectedInventoryItem != BAG_COLUMNS - 1 && _currentSelectedInventoryItem != BAG_COLUMNS * BAG_ROWS - 1)
                {
                    _frameInventorySelected.transform.position += 150 * Vector3.right;
                    _currentSelectedInventoryItem += (int)Sign(xAmount);
                }
                else if (Sign(xAmount) < 0 && _currentSelectedInventoryItem != 0 && _currentSelectedInventoryItem != BAG_COLUMNS)
                {
                    _frameInventorySelected.transform.position -= 150 * Vector3.right;
                    _currentSelectedInventoryItem += (int)Sign(xAmount);
                }
                else if (Sign(yAmount) > 0 && _currentSelectedInventoryItem < BAG_COLUMNS)
                {
                    _frameInventorySelected.transform.position += 200 * Vector3.down;
                    _currentSelectedInventoryItem += (int)Sign(yAmount) * BAG_COLUMNS;
                }
                else if (Sign(yAmount) < 0 && _currentSelectedInventoryItem >= BAG_COLUMNS)
                {
                    _frameInventorySelected.transform.position -= 200 * Vector3.down;
                    _currentSelectedInventoryItem += (int)Sign(yAmount) * BAG_COLUMNS;
                }
            }

            needsRelease = true;
        }
    }

    private int Sign(float number)
    {
        if (number > 0)
            return 1;
        else if (number < 0)
            return -1;
        else return 0;
    }

    public void ButtonReleased()
    {
        needsRelease = false;
    }

    public void OnBagSlotSelected()
    {
        inEquippedSelection = false;
        //ResetSelectionFrames();
        _frameInventorySelected.SetActive(true);
    }

    public void OnInventorySlotSelected()
    {
        _inventory.MoveObjectToBag(_currentSelectedInventoryItem, _currentSelectedBagItem);
        RefreshInventoryUI();
        ResetSelectionFrames();

        inEquippedSelection = true;
        _frameInventorySelected.SetActive(false);
    }

    private void ResetSelectionFrames()
    {
        _currentSelectedInventoryItem = 0;
        _frameInventorySelected.transform.position = _startPositionInventoryFrame;

        _currentSelectedBagItem = 0;
        _frameBagSelected.transform.position = _startPositionBagFrame;
        
    }

    public void RefreshInventoryUI()
    {
        List<InventoryObject> inventoryObjects = _inventory.GetInventoryItems();
        for (int i = 0; i < _inventorySlots.Count; ++i)
        {
            if (!_inventory.IsInventoryEmptyAt(i)) 
            {
                _inventorySlots[i].EnableSlot();
                _inventorySlots[i].LoadItemData(inventoryObjects[i]);
            }
            else
                _inventorySlots[i].DisableSlot();
        }

        List<InventoryObject> bagObjects = _inventory.GetBagItems();
        for (int i = 0; i < _bagSlots.Count; ++i)
        {
            if (!_inventory.IsBagEmptyAt(i))
            {
                _bagSlots[i].EnableSlot();
                _bagSlots[i].LoadItemData(bagObjects[i]);
            }
            else
                _bagSlots[i].DisableSlot();
        }

        Sprite objectSprite = _inventory.GetCurrentBagObjectIcon();
        _inGameEquippedItemIcon.gameObject.SetActive(!ReferenceEquals(objectSprite, null));
        _inGameEquippedItemIcon.sprite = _inventory.GetCurrentBagObjectIcon();
    }

    private void OnItemChange()
    {
        _inGameEquippedItemIcon.sprite = _inventory.GetCurrentBagObjectIcon();
    }
}
