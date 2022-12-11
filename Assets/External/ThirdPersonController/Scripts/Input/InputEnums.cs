using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum INPUT_AXIS
{ 
    LH = 0,
    LV, 
    RH, 
    RV,
    DPH,
    DPV
}

public enum INPUT_BUTTONS
{
    BUTTON_A = 0,
    BUTTON_B,
    BUTTON_X,
    BUTTON_Y,
    BUTTON_RB,
    BUTTON_RT,
    BUTTON_R3,
    BUTTON_LB,
    BUTTON_LT,
    BUTTON_L3,
    BUTTON_START,
    BUTTON_SELECT
}


public enum BUTTON_STATE
{ 
    NOPRESS = 0,
    PRESS,
    HOLD,
    RELEASE,
    HOLD_RELEASE
}
