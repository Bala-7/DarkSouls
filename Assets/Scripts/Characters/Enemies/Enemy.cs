using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum ENEMY_TYPE
{ 
    NORMAL = 0,
    BOSS
}

public abstract class Enemy : GameCharacter
{
    protected Animator _animator;
    protected Transform _body;
    protected NavMeshAgent _navMeshAgent;
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }

    [SerializeField]
    protected ThirdPersonControllerMovement _player;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private EnemyPerformance _performance;
    public EnemyPerformance Performance { get { return _performance; } }

    protected EnemyState _enemyState;
    protected Dictionary<ENEMY_STATE_ID, EnemyState> _states;

    public EnemyState GetState(ENEMY_STATE_ID id) { return _states[id]; }

    [SerializeField]
    protected Projectile _projectilePrefab;
    public Projectile ProjectilePrefab { get { return _projectilePrefab; }  }
    protected List<Projectile> _projectileInstances;
    public void AddToProjectiles(Projectile p) { _projectileInstances.Add(p); }
    public Projectile GetCurrentProjectile() { return _projectileInstances[_projectileInstances.Count - 1]; }

    private Slider _healthSlider;

    #region Souls
    private int _soulsDrop;
    public int SoulsDrop { get { return _soulsDrop; } }

    private GameObject _soulsFXPrefab;
    private string _soulsFXPrefabPath = "Prefabs/FX/FX_Ghost_Dust";
    #endregion

    private float currentHitTime = 0;
    private float minHitTime = 1.0f;
    protected bool CanBeHit { get { return alive && (currentHitTime >= minHitTime); } }

    private ENEMY_TYPE _type;
    public ENEMY_TYPE Type { get { return _type; } }


    #region Audio
    private AudioSource _audioSource;
    [SerializeField] private List<AudioClip> _hitSounds;
    [SerializeField] private List<AudioClip> _deathSounds;
    #endregion

    protected new void Awake()
    {
        _body = transform.Find("Body");
        _player = ThirdPersonControllerMovement.s;
        _animator = _body.GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _states = new Dictionary<ENEMY_STATE_ID, EnemyState>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        _audioSource = GetComponent<AudioSource>();
        if (!_audioSource)
            _audioSource = gameObject.AddComponent<AudioSource>();

        _soulsDrop = Random.Range(20, 100);
        _soulsFXPrefab = Resources.Load<GameObject>(_soulsFXPrefabPath);
        base.Awake();
    }

    protected override void Start()
    {
        GameManager.s.AddToEnemyList(this);
    }

    protected virtual void Update()
    {
        if (alive)
        {
            currentHitTime += Time.deltaTime;


            EnemyState newState = _enemyState.Update(this);
            if (!ReferenceEquals(newState, null))
            {
                _enemyState = newState;
                _enemyState.Enter(this);
            }
        }
    }

    public void SetType(ENEMY_TYPE newType) { _type = newType; }

    protected override void InitializePerformance()
    {
        _performance = _body.gameObject.GetComponent<EnemyPerformance>();
    }

    public float GetCurrentAnimationStateCompletionPercentage()
    {
        float normalizedTime = _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float decimals = normalizedTime - Mathf.Floor(normalizedTime);

        return decimals;
    }


    public override void EnableCurrentWeapons()
    {
        EnemyWeapon[] activeWeapons = weaponsParent.GetComponentsInChildren<EnemyWeapon>();
        foreach (EnemyWeapon weapon in activeWeapons)
        {
            BoxCollider collider = weapon.GetComponent<BoxCollider>();
            collider.enabled = true;
        }
    }

    public override void DisableCurrentWeapons()
    {
        EnemyWeapon[] activeWeapons = weaponsParent.GetComponentsInChildren<EnemyWeapon>();
        foreach (EnemyWeapon weapon in activeWeapons)
        {
            BoxCollider collider = weapon.GetComponent<BoxCollider>();
            collider.enabled = false;
        }
    }

    public void PlayAnimation(string animationName)
    {
        _animator.Play(animationName);
    }

    public bool IsCurrentAnimation(string animationName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    public void SetNavMeshAgentDestination(Vector3 position)
    {
        _navMeshAgent.SetDestination(position);
    }

    public void StopMovement()
    {
        SetNavMeshAgentDestination(transform.position);
    }

    public void LookPlayer()
    {
        Vector3 targetPoint = transform.position - new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(-targetPoint, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2.0f);
    }

    public void Die()
    {
        alive = false;
        ThirdPersonControllerMovement.s.Performance.AddSouls(_soulsDrop);
        AudioClip deathSoundClip = AudioManager.instance.GetRandomSound(_deathSounds);
        if(deathSoundClip)
            AudioManager.instance.PlayAudio(deathSoundClip, _audioSource, 0.15f);
    }

    public override void OnReceiveHit() 
    {
        currentHitTime = 0;
        EventsManager.instance.NotifyEvent(ENEMY_EVENTS.ENEMY_HEALTH_CHANGE, this);
        AudioClip hitSoundClip = AudioManager.instance.GetRandomSound(_hitSounds);
        if(hitSoundClip) 
            AudioManager.instance.PlayAudio(hitSoundClip, _audioSource, 0.15f);
    }

    public void Restart()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        alive = true;

        _performance.Restart(); 

        _enemyState = GetState(ENEMY_STATE_ID.IDLE);
        _enemyState.Enter(this);
    }

    public override void OnCharacterSpawn()
    {
        
    }

    public void SpawnSouls()
    {
        StartCoroutine("SpawnSoulsCoroutine");
    }

    private IEnumerator SpawnSoulsCoroutine()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(_soulsFXPrefab, transform.position, Quaternion.identity);
    }
}
