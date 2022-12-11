using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireMenu : MonoBehaviour
{
    private Transform _selectionBox;
    private Transform _resumeGameText;
    private Transform _quitGameText;

    private PlayerInputObserver _inputObserver;

    private GameObject _mainText;
    private GameObject _background;

    delegate void DelegateMethod();
    private DelegateMethod _currentOption;

    private bool IsShowingMenu() { return _resumeGameText.gameObject.activeSelf; }

    private void Awake()
    {
        _selectionBox = transform.Find("SelectionBox");
        _resumeGameText = transform.Find("ResumeGameText");
        _quitGameText = transform.Find("QuitText");

        _mainText = transform.Find("MainText").gameObject;
        _background = transform.Find("Background").gameObject;

        _selectionBox.gameObject.SetActive(false);
        _resumeGameText.gameObject.SetActive(false);
        _quitGameText.gameObject.SetActive(false);
        _mainText.SetActive(false);
        _background.SetActive(false);

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

        _selectionBox.position = _resumeGameText.position;
        _currentOption = OnResumeGameButton;

        Hide();
    }

    private void OnNewInput(NewPlayerInput input)
    {
        if (GameManager.s.IsInState(GAME_STATE.BONFIRE_MENU)) 
        {
            if (input.AxisStates[(int)INPUT_AXIS.DPV].Item2 <= -1)
            {   // Up 
                _selectionBox.position = _resumeGameText.position;
                _currentOption = OnResumeGameButton;

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

    private void OnResumeGameButton()
    {
        // TODO: Resume game
        Time.timeScale = 1;
        Hide();
        GameManager.s.SetCurrentState(GAME_STATE.GAMEPLAY);
    }

    private void OnQuitGameButton()
    {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    public void Show()
    {
        _selectionBox.gameObject.SetActive(true);
        _resumeGameText.gameObject.SetActive(true);
        _quitGameText.gameObject.SetActive(true);
        _mainText.SetActive(true);
        _background.SetActive(true);
    }

    public void Hide()
    {
        _selectionBox.gameObject.SetActive(false);
        _resumeGameText.gameObject.SetActive(false);
        _quitGameText.gameObject.SetActive(false);
        _mainText.SetActive(false);
        _background.SetActive(false);
    }
}
