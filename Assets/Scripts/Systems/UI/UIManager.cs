using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager s;

    private PlayerObserver _playerObserver;
    private PlayerInputObserver _playerInputObserver;

    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Slider _staminaSlider;

    private UIState _currentState;

    private ButtonMessage _buttonMessage;
    public ButtonMessage ButtonMessage { get { return _buttonMessage; } }

    private InfoMessage _infoMessage;
    public InfoMessage InfoMessage { get { return _infoMessage; } }

    private LockEnemyIcon _lockIcon;

    private SoulsCounter _soulsCounter;

    private BigEventUI _bigEventUI;

    private GameObserver _gameObserver;

    private Image _blackScreen;
    private float _fadeSpeed = 1f;
    public bool IsOnBlackScreen { get { return _blackScreen.color.a >= 1f; } }

    private BonfireMenu _bonfireMenu;
    private MainMenu _mainMenu;


    #region Enemy Health Bars
    private EnemyObserver _enemyObserver;
    [SerializeField] private EnemyHealthBar _enemyHealthSliderPrefab;
    private int _enemyHealthBarPoolSize = 10;
    private Stack<EnemyHealthBar> _enemyHealthBarPool;
    private Transform _enemyHealthBarsParent;
    private List<EnemyHealthBar> _activeEnemyHealthBars;
    #endregion

    #region BossHealth
    private BossObserver _bossObserver;
    private Transform _bossHealthBarParent;
    private Slider _bossHealthBarSlider;
    private EnemyHealthBar _bossHealthBar;
    #endregion

    #region Armor UI
    private ArmorUI _armorUI;
    private bool upDownPressed = false;
    public ArmorUI ArmorUI { get { return _armorUI; } }
    #endregion

    #region InventoryUI
    private InventoryUI _inventoryUI;
    public InventoryUI InventoryUI { get { return _inventoryUI; } }
    #endregion

    private void Awake()
    {
        if (!s) s = this;

        _buttonMessage = GetComponentInChildren<ButtonMessage>();
        _infoMessage = GetComponentInChildren<InfoMessage>();
        _lockIcon = GetComponentInChildren<LockEnemyIcon>();
        _soulsCounter = GetComponentInChildren<SoulsCounter>();
        _bigEventUI = GetComponentInChildren<BigEventUI>();

        _blackScreen = transform.Find("BlackScreen").GetComponentInChildren<Image>();

        _bonfireMenu = transform.Find("BonfireMenu").GetComponent<BonfireMenu>();
        _mainMenu = transform.Find("MainMenu").GetComponent<MainMenu>();

        _playerObserver = new PlayerObserver(new List<PLAYER_EVENTS>() { PLAYER_EVENTS.PLAYER_HEALTH_CHANGE, PLAYER_EVENTS.PLAYER_STAMINA_CHANGE }, new List<PlayerObserver.OnNotifyDelegate>() { OnPlayerHealthChange, OnPlayerStaminaChange });
        EventsManager.instance.RegisterObserver(_playerObserver);

        _enemyObserver = new EnemyObserver(new List<ENEMY_EVENTS>() { ENEMY_EVENTS.ENEMY_HEALTH_CHANGE, ENEMY_EVENTS.ENEMY_DEAD }, new List<EnemyObserver.OnNotifyDelegate>() { OnEnemyHealthChange, OnEnemyDead });
        EventsManager.instance.RegisterObserver(_enemyObserver);

        _bossObserver = new BossObserver(new List<BOSS_EVENTS>() { BOSS_EVENTS.BOSS_ENTER },
            new List<BossObserver.OnNotifyDelegate>() { OnPlayerEnterBoss });
        EventsManager.instance.RegisterObserver(_bossObserver);

        _gameObserver = new GameObserver(new List<GAME_EVENTS>() { GAME_EVENTS.PLAYER_DEAD, GAME_EVENTS.START_GAME, GAME_EVENTS.BOSS_DEFEAT },
            new List<GameObserver.OnNotifyDelegate>() { OnPlayerDead, OnGameRestart, OnBossDead });
        EventsManager.instance.RegisterObserver(_gameObserver);


        _bossHealthBarParent = transform.Find("BossHealthBar");
        _bossHealthBarSlider = _bossHealthBarParent.GetComponentInChildren<Slider>();
        _bossHealthBar = _bossHealthBarSlider.gameObject.GetComponent<EnemyHealthBar>();
        _bossHealthBarParent.gameObject.SetActive(false);

        _armorUI = transform.Find("ArmorUI").GetComponent<ArmorUI>();
        _inventoryUI = transform.Find("InventoryUI").GetComponent<InventoryUI>();

        _playerInputObserver = new PlayerInputObserver(new List<PLAYER_INPUT>() {
            PLAYER_INPUT.PLAYER_INPUT_NEWINPUT
        },
        new List<PlayerInputObserver.OnNotifyDelegate> {
                OnNewInput
        });

        EventsManager.instance.RegisterObserver(_playerInputObserver);

        _currentState = new UIGameplayState();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateEnemyHealthBarPool();
        _currentState.OnEnter();
    }

    // Update is called once per frame
    void Update()
    {
        UIState newState = _currentState.OnUpdate();
        if (newState != _currentState)
        {
            _currentState = newState;
            _currentState.OnEnter();
        }
    }

    private void OnPlayerHealthChange(float newValue, float currentValue, float maxValue)
    {
        _healthSlider.value = newValue / maxValue;
    }

    private void OnPlayerStaminaChange(float newValue, float currentValue, float maxValue)
    {
        _staminaSlider.value = newValue / maxValue;
    }

    private void CreateEnemyHealthBarPool()
    {
        _enemyHealthBarsParent = transform.Find("EnemyHealthBars");
        _enemyHealthBarPool = new Stack<EnemyHealthBar>();
        _activeEnemyHealthBars = new List<EnemyHealthBar>();

        for (int i = 0; i < _enemyHealthBarPoolSize; ++i)
        {
            EnemyHealthBar s = Instantiate<EnemyHealthBar>(_enemyHealthSliderPrefab, _enemyHealthBarsParent);
            s.gameObject.SetActive(false);
            _enemyHealthBarPool.Push(s);
        }
    }

    #region Enemy Health Bars
    private EnemyHealthBar ActivateHealthSliderOnEnemy(Enemy e)
    {
        EnemyHealthBar healthBar = null;
        if (_enemyHealthBarPool.Count > 0)
        {
            healthBar = _enemyHealthBarPool.Pop();
            healthBar.gameObject.SetActive(true);
            healthBar.LinkToEnemy(e);
            _activeEnemyHealthBars.Add(healthBar);
        }
        return healthBar;
    }

    private void DeactivateHealthSliderOnEnemy(EnemyHealthBar healthBar)
    {
        healthBar.Unlink();
        healthBar.gameObject.SetActive(false);
        _activeEnemyHealthBars.Remove(healthBar);
        _enemyHealthBarPool.Push(healthBar);
    }

    private void OnEnemyHealthChange(Enemy enemy)
    {
        bool foundInActiveBars = false;
        foreach (EnemyHealthBar healthBar in _activeEnemyHealthBars)
        {
            Enemy e = healthBar.Enemy;
            if (!ReferenceEquals(e, null))
            {
                if (ReferenceEquals(enemy, e))
                {
                    healthBar.UpdateValue();
                    foundInActiveBars = true;
                    break;
                }
            }
        }

        if (!foundInActiveBars)
        {
            EnemyHealthBar newHealthBar = ActivateHealthSliderOnEnemy(enemy);
            newHealthBar.UpdateValue();
        }
    }

    private void OnEnemyDead(Enemy enemy)
    {
        foreach (EnemyHealthBar healthBar in _activeEnemyHealthBars)
        {
            Enemy e = healthBar.Enemy;
            if (!ReferenceEquals(e, null))
            {
                if (ReferenceEquals(enemy, e))
                {
                    DeactivateHealthSliderOnEnemy(healthBar);
                    break;
                }
            }
        }

        _soulsCounter.UpdateSouls();
    }
    #endregion

    #region Boss UI
    private void OnPlayerEnterBoss(Enemy e)
    {
        _bossHealthBarParent.gameObject.SetActive(true);
        _bossHealthBar.LinkToEnemy(e);
        _activeEnemyHealthBars.Add(_bossHealthBar);

    }

    private void OnBossDead()
    {
        _bigEventUI.DisplayVictoryMessage();
        FadeOut();
        Action showMainMenuAction = () => _mainMenu.Show();
        StartCoroutine(ExecuteAfterSeconds(showMainMenuAction, 4.0f));
        //_mainMenu.Show();
    }

    public IEnumerator ExecuteAfterSeconds(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action();
    }

    #endregion

    private void OnNewInput(NewPlayerInput input)
    {
        UIState prevState = _currentState;
        _currentState = _currentState.OnInput(input);
        if (prevState != _currentState)
        {
            prevState.OnExit();
            _currentState.OnEnter();
        }

        /*foreach (Tuple<BUTTON_STATE, float> button in input.ButtonStates)
        { 
            if(button.Item1 != BUTTON_STATE.NOPRESS)
                Debug.Log(button.Item1.ToString() + " " + button.Item2.ToString());
        }*/
    }

    public void OnEnemyLocked(Enemy e)
    {
        _lockIcon.OnEnemyLocked(e);
    }

    public void OnEnemyUnlocked()
    {
        _lockIcon.OnEnemyUnlocked();
    }

    public void HideAllMenus()
    {
        _armorUI.gameObject.SetActive(false);
        _inventoryUI.gameObject.SetActive(false);
        try { ThirdPersonControllerMovement.s.EnableMovement(); } catch (Exception e) { Debug.LogError("CHECK THIS!"); }
    }

    private void OnPlayerDead()
    {
        _bigEventUI.DisplayDeathMessage();
        FadeOut();
    }



    private void OnGameRestart()
    {
        _bigEventUI.Restart();
        GameManager.s.ResetGame();
        FadeIn();
    }

    public void FadeOut()
    {
        StartCoroutine("FadeOutCoroutine");
    }

    public void FadeIn()
    {
        StartCoroutine("FadeInCoroutine");
    }

    private IEnumerator FadeOutCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        Color c = _blackScreen.color;
        _blackScreen.color = new Color(c.r, c.g, c.b, 0);
        while (_blackScreen.color.a <= 1f)
        {
            c = _blackScreen.color;
            _blackScreen.color = new Color(c.r, c.g, c.b, c.a += _fadeSpeed * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator FadeInCoroutine()
    {
        yield return new WaitForSeconds(5.0f);
        Color c = _blackScreen.color;
        _blackScreen.color = new Color(c.r, c.g, c.b, 1f);
        while (_blackScreen.color.a >= 0)
        {
            c = _blackScreen.color;
            _blackScreen.color = new Color(c.r, c.g, c.b, c.a -= _fadeSpeed * Time.deltaTime);
            yield return null;
        }
        yield return null;
    }

    public void OnEnterBonfire()
    {
        _bonfireMenu.Show();
        GameManager.s.SetCurrentState(GAME_STATE.BONFIRE_MENU);
    }

    public void OnExitBonfire()
    {
        Time.timeScale = 1;
        _bonfireMenu.Hide();
    }
}
