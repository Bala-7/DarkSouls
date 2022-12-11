using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GAME_STATE
{ 
    GAMEPLAY = 0,
    BONFIRE_MENU,
    MAIN_MENU
}

public class GameManager : MonoBehaviour
{
    public static GameManager s;

    private List<Enemy> _enemyList;

    private CharacterSpawnPoint _playerSpawn;

    private GAME_STATE _state;
    

    private void Awake()
    {
        ApplyGameSettings();

        s = this;

        _enemyList = new List<Enemy>();
        _playerSpawn = FindObjectOfType<CharacterSpawnPoint>();
    }

    public void AddToEnemyList(Enemy enemy)
    {
        _enemyList.Add(enemy);
    }

    public void StartNewGame()
    {
        _state = GAME_STATE.GAMEPLAY;
        DungeonGenerator.s.DeleteDungeon();
        DungeonGenerator.s.GenerateDungeon();
        _playerSpawn.SpawnCharacter();
    }

    public void SetCurrentState(GAME_STATE newState) { _state = newState; }

    public bool IsInState(GAME_STATE state) { return _state == state; }

    public void ResetGame()
    {
        ThirdPersonControllerMovement.s.Restart();

        foreach (Enemy enemy in _enemyList)
        {
            enemy.Restart();
        }

        EventsManager.instance.NotifyEvent(GAME_EVENTS.START_GAME);
    }

    private void ApplyGameSettings()
    {
        Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
    }
}
