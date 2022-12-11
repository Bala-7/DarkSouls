using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NewPlayerInput
{
    private List<Tuple<BUTTON_STATE, float>> buttonStates;
    private List<Tuple<INPUT_AXIS, float>> axisStates;

    public static List<string> ButtonNames = new List<string> { "BUTTON_A", "BUTTON_B", "BUTTON_X", "BUTTON_Y", "BUTTON_R1", "BUTTON_R2", "BUTTON_R3",
            "BUTTON_L1", "BUTTON_L2", "BUTTON_L3", "BUTTON_START", "BUTTON_SELECT" };

    public static List<string> AxisNames = new List<string> { "LeftStick_Horizontal", "LeftStick_Vertical", "Mouse X", "Mouse Y", "DPAD_HAxis", "DPAD_VAxis" };

    public static float HOLD_TIME = 0.15f;

    public List<Tuple<BUTTON_STATE, float>> ButtonStates { get { return buttonStates; } }
    public List<Tuple<INPUT_AXIS, float>> AxisStates { get { return axisStates; } }

    private float _timestamp;
    public float TimeStamp { get { return _timestamp; } }

    public NewPlayerInput(float timestamp)
    {
        _timestamp = timestamp;
        buttonStates = new List<Tuple<BUTTON_STATE, float>>();
        axisStates = new List<Tuple<INPUT_AXIS, float>>();

        for (int i = 0; i < ButtonNames.Count; ++i)
        {
            BUTTON_STATE state = GetButtonStateFromInputModule(ButtonNames[i]);

            Tuple<BUTTON_STATE, float> buttonState = new Tuple<BUTTON_STATE, float>(state, 0);
            buttonStates.Add(buttonState);
        }

        for (int i = 0; i < AxisNames.Count; ++i)
        {
            Tuple<INPUT_AXIS, float> axisValue = new Tuple<INPUT_AXIS, float>((INPUT_AXIS)i, Input.GetAxis(AxisNames[i]));
            axisStates.Add(axisValue);
        }
        
    }

    public BUTTON_STATE GetButtonState(INPUT_BUTTONS button)
    {
        return ButtonStates[(int)button].Item1;
    }

    private BUTTON_STATE GetButtonStateFromInputModule(string buttonName)
    {
        try 
        {
            if (Input.GetButtonUp(buttonName))
                return BUTTON_STATE.RELEASE;
            else if (Input.GetButtonDown(buttonName))
                return BUTTON_STATE.PRESS;
            else if (Input.GetButton(buttonName))
                return BUTTON_STATE.HOLD;
            else return BUTTON_STATE.NOPRESS;
        } 
        catch (ArgumentException e) 
        {
            Debug.LogWarning("Input button " + buttonName + " not defined. Please add it to Project Settings.");
            return BUTTON_STATE.NOPRESS;
        }
        
    }
}

public struct PlayerInput
{
    public float hInput;
    public float vInput;

    public bool runPress;
    public bool runRelease;

    public bool rollPress;
    public bool rollRelease;

    public bool attackA;
    public bool attackARelease;

    public bool useItem;
    public bool nextItem;


    public PlayerInput(float h, float v, bool run, bool runRelease, bool roll, bool rollRelease, bool attackA, bool attackARelease, bool useItem, bool nextItem)
    {
        hInput = h;
        vInput = v;
        runPress = run;
        this.runRelease = runRelease;
        rollPress = roll;
        this.rollRelease = rollRelease;
        this.attackA = attackA;
        this.attackARelease = attackARelease;
        this.useItem = useItem;
        this.nextItem = nextItem;
    }
}

public class ThirdPersonInput : MonoBehaviour
{
    private NewPlayerInput previousInput, currentInput;

    private float _prevHAxisPad, _prevVAxisPad;

    private void Update()
    {
        //GetPlayerInput();
        GetPlayerNewInput();
    }

    private void GetPlayerNewInput()
    {
        currentInput = new NewPlayerInput(Time.time);
        HandleSpecialCases();
        if(MustNotifyInputChange())
            EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_NEWINPUT, currentInput);

        previousInput = currentInput;
    }

    private void GetPlayerInput()
    {
        // Gets the input
        float hInput = Input.GetAxis("LeftStick_Horizontal");
        float vInput = Input.GetAxis("LeftStick_Vertical");
        //crouch = (Input.GetButtonDown("Crouch")) ? !crouch : crouch;
        bool runPress = Input.GetButton("BUTTON_A");
        bool rollPress = Input.GetButton("BUTTON_B");

        bool runRelease = Input.GetButtonUp("BUTTON_A");
        bool rollRelease = Input.GetButtonUp("BUTTON_B");

        bool attackA = Input.GetButton("BUTTON_R1");
        bool attackARelease = Input.GetButtonUp("BUTTON_R1");

        bool useItem = Input.GetButtonDown("BUTTON_X");

        bool menu = Input.GetButtonDown("BUTTON_START");
        bool menuSwitch = Input.GetButtonDown("BUTTON_SELECT");
        bool menuForward = Input.GetButtonDown("BUTTON_R1");
        bool menuBack = Input.GetButtonDown("BUTTON_L1");
        float vAxisPad = Input.GetAxis("DPAD_VAxis");
        float hAxisPad = Input.GetAxis("DPAD_HAxis");

        bool padDownPress = vAxisPad >= 1;
        bool padUpPress = vAxisPad <= -1;

        bool interact = Input.GetButtonDown("Interact");
        if (interact) 
        {
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_INTERACT_BUTTON, null);
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_A_BUTTON, null);
        }

        if (menu) 
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_START_BUTTON, null);
        if (menuSwitch)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_SELECT_BUTTON, null);
        if (menuBack)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_L1_BUTTON, null);
        if(menuForward)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_R1_BUTTON, null);
        if(padDownPress)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_DOWN_PRESS, null);
        if (padUpPress)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_UP_PRESS, null);
        if (Mathf.Abs(vAxisPad) < 0.2f && Mathf.Abs(_prevVAxisPad) >= 0.2f) 
        {
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_DOWN_RELEASE, null);
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_UP_RELEASE, null);
        }
        _prevVAxisPad = vAxisPad;

        if (hAxisPad >= 1)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_LEFT_PRESS, null);
        if (hAxisPad <= -1)
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_RIGHT_PRESS, null);
        if (Mathf.Abs(hAxisPad) < 0.2f && Mathf.Abs(_prevHAxisPad) >= 0.2f)
        {
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_RIGHT_RELEASE, null);
            //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_PAD_LEFT_RELEASE, null);
        }
        _prevHAxisPad = hAxisPad;

        

        PlayerInput input = new PlayerInput(hInput, vInput, runPress, runRelease, rollPress, rollRelease, attackA, attackARelease, useItem, padDownPress);


        //EventsManager.instance.NotifyEvent(PLAYER_INPUT.PLAYER_INPUT_BUTTON, input);
    }

    /*
    *   This method manages special input cases, such as detecting when the player is holding a button.
    *   
    **/
    private void HandleSpecialCases()
    {
        try
        {
            for (int i = 0; i < currentInput.ButtonStates.Count; ++i)
            {
                if (currentInput.ButtonStates[i].Item1 == BUTTON_STATE.HOLD)
                {
                    HandleButtonHold(i);
                }
                else if (currentInput.ButtonStates[i].Item1 == BUTTON_STATE.RELEASE)
                {   // Distinguish between RELEASE and HOLD_RELEASE
                    if (previousInput.ButtonStates[i].Item1 == BUTTON_STATE.HOLD && previousInput.ButtonStates[i].Item2 >= NewPlayerInput.HOLD_TIME)
                        currentInput.ButtonStates[i] = new Tuple<BUTTON_STATE, float>(BUTTON_STATE.HOLD_RELEASE, currentInput.ButtonStates[i].Item2);
                }
            }
        }
        catch (Exception e)
        {
            
        }
    }

    private void HandleButtonHold(int index)
    {
        float deltaTime = currentInput.TimeStamp - previousInput.TimeStamp;
        if (previousInput.ButtonStates[index].Item1 == BUTTON_STATE.HOLD)
        {
            currentInput.ButtonStates[index] = new Tuple<BUTTON_STATE, float>(currentInput.ButtonStates[index].Item1, previousInput.ButtonStates[index].Item2 + deltaTime);
        }
        else
        {
            currentInput.ButtonStates[index] = new Tuple<BUTTON_STATE, float>(currentInput.ButtonStates[index].Item1, deltaTime);
        }
    }

    private bool MustNotifyInputChange()
    {
        try
        {
            for (int i = 0; i < currentInput.ButtonStates.Count; ++i)
            {
                if (currentInput.ButtonStates[i].Item2 != previousInput.ButtonStates[i].Item2)
                    return true;
            }

            for (int i = 0; i < currentInput.AxisStates.Count; ++i)
            {
                if (currentInput.AxisStates[i].Item2 != previousInput.AxisStates[i].Item2)
                    return true;
            }
        }
        catch (Exception e)
        {
            return false;
        }

        return false;
    }
}
