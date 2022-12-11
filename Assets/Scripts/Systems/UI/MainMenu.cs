using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private Transform _selectionBox;
    private Transform _newGameText;
    private Transform _quitGameText;

    private GameObject _background;
    private GameObject _mainText;

    private GameObject _assetsText;
    private GameObject _copyrightText;

    private PlayerInputObserver _inputObserver;

    delegate void DelegateMethod();
    private DelegateMethod _currentOption;

    private bool IsShowingMenu() { return gameObject.activeSelf; }

    private void Awake()
    {

        _selectionBox = transform.Find("SelectionBox");
        _newGameText = transform.Find("NewGameText");
        _quitGameText = transform.Find("QuitText");

        _mainText = transform.Find("MainText").gameObject;
        _background = transform.Find("Background").gameObject;

        _assetsText = transform.Find("AssetsText").gameObject;
        _copyrightText = transform.Find("CopyrightText").gameObject;

        GameManager.s.SetCurrentState(GAME_STATE.MAIN_MENU);

        _inputObserver = new PlayerInputObserver(new List<PLAYER_INPUT>() {
                PLAYER_INPUT.PLAYER_INPUT_NEWINPUT
            },
            new List<PlayerInputObserver.OnNotifyDelegate> {
                    OnNewInput
            });
        
    }

    private void Start()
    {
        EventsManager.instance.RegisterObserver(_inputObserver);

        _selectionBox.position = _newGameText.position;
        _currentOption = OnNewGameButton;
    }
    private void OnNewInput(NewPlayerInput input)
    {
        if (GameManager.s.IsInState(GAME_STATE.MAIN_MENU)) 
        {
            if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 <= -1)
            {   // Up 
                _selectionBox.position = _newGameText.position;
                _currentOption = OnNewGameButton;

            }
            else if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 >= 1)
            {   // Down 
                _selectionBox.position = _quitGameText.position;
                _currentOption = OnQuitGameButton;
            }
            else if (input.GetButtonState(INPUT_BUTTONS.BUTTON_A) == BUTTON_STATE.RELEASE)
            {   // Press A
                _currentOption();
            }
        }
        
    }

    private void OnNewGameButton()
    {
        Debug.Log("Starting new game.");
        
        GameManager.s.StartNewGame();
        Hide();
        UIManager.s.FadeIn();
    }

    private void OnQuitGameButton()
    {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    public void Show()
    {
        _selectionBox.gameObject.SetActive(true);
        _newGameText.gameObject.SetActive(true);
        _quitGameText.gameObject.SetActive(true);

        _mainText.SetActive(true);
        _background.SetActive(true);
        _assetsText.SetActive(true);
        _copyrightText.SetActive(true);

        GameManager.s.SetCurrentState(GAME_STATE.MAIN_MENU);
    }

    public void Hide()
    {
        _selectionBox.gameObject.SetActive(false);
        _newGameText.gameObject.SetActive(false);
        _quitGameText.gameObject.SetActive(false);

        _mainText.SetActive(false);
        _background.SetActive(false);
        _assetsText.SetActive(false);
        _copyrightText.SetActive(false);
    }
}
