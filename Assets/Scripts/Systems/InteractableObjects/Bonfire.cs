using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : InteractableObject
{
    protected override void OnPlayerInput(NewPlayerInput input)
    {
        if (!GameManager.s.IsInState(GAME_STATE.GAMEPLAY))
            return;

        if ((input.GetButtonState(INPUT_BUTTONS.BUTTON_A) == BUTTON_STATE.RELEASE) && canBeInteracted)
        {
            Time.timeScale = 0.05f;
            UIManager.s.OnEnterBonfire();
            canBeInteracted = false;
        }
    }

    private new void Awake()
    {
        base.Awake();
        inRangeMessage = "Bonfire.";
        nMaxInteractions = int.MaxValue;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
