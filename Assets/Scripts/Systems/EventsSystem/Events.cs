public enum EVENTS 
{
    
}

public enum GAME_EVENTS
{ 
    PLAYER_SPAWN = 0,
    PLAYER_DEAD,
    BOSS_ENTER,
    BOSS_DEFEAT,
    RESET_GAME,
    START_GAME
}

public enum BOSS_EVENTS
{ 
    BOSS_ENTER = 0,
    BOSS_DEFEAT
}

public enum PLAYER_EVENTS
{
    PLAYER_HEALTH_CHANGE = 0,
    PLAYER_STAMINA_CHANGE,
    BOSS_ENTER,
    BOSS_DEFEAT
}

public enum PLAYER_INPUT
{ 
    PLAYER_INPUT_BUTTON = 0,
    PLAYER_INPUT_START_BUTTON,
    PLAYER_INPUT_SELECT_BUTTON,
    PLAYER_INPUT_R1_BUTTON,
    PLAYER_INPUT_L1_BUTTON,
    PLAYER_INPUT_A_BUTTON,
    PLAYER_INPUT_B_BUTTON,
    PLAYER_INPUT_X_BUTTON,
    PLAYER_INPUT_Y_BUTTON,
    PLAYER_INPUT_PAD_UP_PRESS,
    PLAYER_INPUT_PAD_UP_RELEASE,
    PLAYER_INPUT_PAD_DOWN_PRESS,
    PLAYER_INPUT_PAD_DOWN_RELEASE,
    PLAYER_INPUT_PAD_LEFT_PRESS,
    PLAYER_INPUT_PAD_LEFT_RELEASE,
    PLAYER_INPUT_PAD_RIGHT_PRESS,
    PLAYER_INPUT_PAD_RIGHT_RELEASE,
    PLAYER_INPUT_INTERACT_BUTTON,
    PLAYER_INPUT_NEWINPUT
}

public enum UI_EVENTS
{
    PLAYER_EQUIPPED_ITEM_CHANGE = 0
}

public enum ENEMY_EVENTS
{ 
    ENEMY_HEALTH_CHANGE = 0,
    ENEMY_DEAD
}