using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : InteractableObject
{
    private List<InventoryObject> _drops;
    private Inventory _inventory;

    private new void Awake()
    {
        base.Awake();
        _inventory = ThirdPersonControllerMovement.s.GetComponentInChildren<Inventory>();
        inRangeMessage = "Open Chest.";
        nMaxInteractions = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        _drops = new List<InventoryObject>();
        _drops.Add(new InventoryObject(_inventory, "Small Potion"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void OnPlayerInput(NewPlayerInput input)
    {
        if ((input.GetButtonState(INPUT_BUTTONS.BUTTON_A) == BUTTON_STATE.RELEASE) && canBeInteracted)
        {
            nInteractions++;
            _inventory.PickDrop(_drops);
            ShowDropsInfoMessage();

            canBeInteracted = false;
        }
    }

    private void ShowDropsInfoMessage()
    {
        foreach (InventoryObject inventoryObject in _drops)
            UIManager.s.InfoMessage.AddMessageToList(inventoryObject.GetName());
        UIManager.s.InfoMessage.SetShowingMessage(true);
        UIManager.s.InfoMessage.ClearMessageList();
    }

}
