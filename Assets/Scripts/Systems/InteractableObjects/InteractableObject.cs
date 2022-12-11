using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class InteractableObject : MonoBehaviour
{
    private Collider _collider;
    private PlayerInputObserver _inputObserver;
    protected bool canBeInteracted = false;
    protected int nInteractions;
    protected int nMaxInteractions;
    protected string inRangeMessage;

    protected virtual void Awake()
    {
        nInteractions = 0;
        _collider = GetComponent<Collider>();
        _inputObserver = new PlayerInputObserver(new List<PLAYER_INPUT>() { PLAYER_INPUT.PLAYER_INPUT_NEWINPUT },
            new List<PlayerInputObserver.OnNotifyDelegate>() { OnPlayerInput });
        EventsManager.instance.RegisterObserver(_inputObserver);
    }

    private void OnTriggerEnter(Collider other)
    {
        ThirdPersonControllerMovement player = other.gameObject.GetComponent<ThirdPersonControllerMovement>();
        if(!ReferenceEquals(player, null) && nInteractions < nMaxInteractions) 
        {
            canBeInteracted = true;
            UIManager.s.ButtonMessage.SetNewMessage(ButtonMessage.CONTROLLER_BUTTONS.BUTTON_A, inRangeMessage);
            UIManager.s.ButtonMessage.SetShowingMessage(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        ThirdPersonControllerMovement player = other.gameObject.GetComponent<ThirdPersonControllerMovement>();
        if (!ReferenceEquals(player, null))
        {
            canBeInteracted = false;
            UIManager.s.ButtonMessage.SetShowingMessage(false);
        }
    }
    protected abstract void OnPlayerInput(NewPlayerInput input);

}
