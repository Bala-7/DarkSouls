using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.Serialization;

public class ThirdPersonControllerMovement : GameCharacter
{
    public static ThirdPersonControllerMovement s;

    private Rigidbody _rb;

    #region Camera
    private Camera _cam;
    public Camera Camera { get { return _cam; } }
    private ThirdPersonCameraMovement _cm;
    private Vector3 camFwd;
    #endregion

    #region Movement
    private PlayerInputObserver _playerInputObserver;

    private bool movementEnabled = true;
    public bool IsMovementEnabled { get { return movementEnabled; } }
    [FormerlySerializedAs("walk_speed")] [Range(1.0f, 10.0f)]
    public float walkSpeed;
    [FormerlySerializedAs("run_speed_multiplier")] [Range(1.0f, 2.0f)]
    public float runSpeedMultiplier = 1.0f;
    [FormerlySerializedAs("crouch_walk_speed_multiplier")] [Range(0.0f, 1.0f)]
    private float crouchWalkSpeedMultiplier;
    [FormerlySerializedAs("crouch_run_speed_multiplier")] [Range(0.0f, 1.0f)]
    private float crouchRunSpeedMultiplier;

    [Range(1.0f, 10.0f)]
    public float backwards_walk_speed;
    [Range(1.0f, 10.0f)]
    public float strafe_speed;

    [FormerlySerializedAs("rotation_speed")] [Range(0.1f, 1.5f)]
    public float rotationSpeed;

    [FormerlySerializedAs("jump_force")] [Range(2.0f, 10.0f)]
    private float jumpForce;

    private Vector3 move = Vector3.zero;

    public bool IsMoving { get { return move != Vector3.zero; } }
    #endregion

    #region Rolling

    private Vector3 _rollDirection;
    private float _rollMultiplierBase = 5f;
    #endregion

    #region Animations
    private MyTPCharacter tpc;
    public MyTPCharacter TPC { get { return tpc; } }
    private float animFreeLookBlend = 0f;
    private float animLockViewBlendX = 0f;
    private float animLockViewBlendY = 0f;
    #endregion

    #region Audio
    private ThirdPersonCharacterAudio _audio;
    public ThirdPersonCharacterAudio Audio { get { return _audio; } }
    #endregion

    #region Attacking
    bool attackA = false;
    #endregion

    #region Input
    private float hInput;
    private float vInput;

    private bool run = false;
    private bool roll = false;

    private NewPlayerInput _lastInput;
    public NewPlayerInput LastInput { get { return _lastInput; } }
    #endregion

    #region Inventory
    private Inventory _inventory;
    public Inventory Inventory { get { return _inventory; } }
    #endregion

    #region States
    private PlayerState _playerState;
    #endregion

    #region Dark Souls Specific Attributes
    private Enemy _lockedEnemy;

    private CharacterEquipment _equipment;
    private CharacterAttributes _attributes;
    private PlayerPerformance _performance;
    public PlayerPerformance Performance { get { return _performance; } }
    public CharacterEquipment Equipment { get { return _equipment; } }
    #endregion

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private new void Awake()
    {
        s = this;
        _cm = GetComponent<ThirdPersonCameraMovement>();
        _cam = _cm.GetCamera();
        _rb = GetComponent<Rigidbody>();
        InitializeModelInfo();

        _inventory = tpc.GetComponent<Inventory>();
        _audio = GetComponent<ThirdPersonCharacterAudio>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (!_cm.lookAt)
            _cm.lookAt = transform;
        //InitializeAnimator();

        base.Awake();
    }

    protected override void InitializePerformance()
    {
        _performance = tpc.gameObject.GetComponent<PlayerPerformance>();
    }

    public void InitializeModelInfo() {
        tpc = FindObjectOfType<MyTPCharacter>();
        _equipment = tpc.GetComponent<CharacterEquipment>();
        _attributes = tpc.GetComponent<CharacterAttributes>();

        InitializeAnimator();
    }

    private void InitializeAnimator()
    {
        tpc.AssignAnimator();
        if (_cm.type == ThirdPersonCameraMovement.CAMERA_TYPE.FREE_LOOK)
        {
            tpc.GetFullBodyAnimator().Play("FreeLookBlendTree");
        }
        else
        {
            tpc.GetFullBodyAnimator().Play("LockViewBlendTree");
        }
    }

    // Start is called before the first frame update
    private new void Start()
    {
        base.Start();
        _playerInputObserver = new PlayerInputObserver(new List<PLAYER_INPUT>() { PLAYER_INPUT.PLAYER_INPUT_NEWINPUT },
            new List<PlayerInputObserver.OnNotifyDelegate>() { OnInputEvent });
        EventsManager.instance.RegisterObserver(_playerInputObserver);

        Cursor.lockState = CursorLockMode.Locked;
        _cm.SetCameraToOrigin();

        DisableCurrentWeapons();

        _playerState = new PlayerEmptyState();
        _playerState.Enter(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        PlayerState newState = _playerState.Update(this);

        if (!ReferenceEquals(newState, null))
        {
            _playerState = newState;
            _playerState.Enter(this);
        }
    }


    public void MovePlayer(float hInput, float vInput, bool run) {
        // Calculate camera relative directions to move:
        camFwd = Vector3.Scale(_cam.transform.forward, new Vector3(1, 1, 1)).normalized;
        Vector3 camFlatFwd = Vector3.Scale(_cam.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 flatRight = new Vector3(_cam.transform.right.x, 0, _cam.transform.right.z);

        Vector3 m_CharForward = Vector3.Scale(camFlatFwd, new Vector3(1, 0, 1)).normalized;
        Vector3 m_CharRight = Vector3.Scale(flatRight, new Vector3(1, 0, 1)).normalized;


        // Draws a ray to show the direction the player is aiming at
        //Debug.DrawLine(transform.position, transform.position + camFwd * 5f, Color.red);

        // Gets terrain height
        int terrainLayerMask = 1 << 6;

        // This would cast rays only against colliders in layer 8.
        
        RaycastHit hit;
        float newHeight = transform.position.y;
        bool falling = false;
        float rayDistance = 1.2f;

        bool frontRayDown = Physics.Raycast(tpc.transform.position + Vector3.up + .5f * tpc.transform.forward, transform.TransformDirection(Vector3.down), out hit, rayDistance, terrainLayerMask);
        bool backRayDown = Physics.Raycast(tpc.transform.position + Vector3.up - .5f * tpc.transform.forward, transform.TransformDirection(Vector3.down), out hit, rayDistance, terrainLayerMask);
        bool midRayDown = Physics.Raycast(tpc.transform.position + Vector3.up, transform.TransformDirection(Vector3.down), out hit, rayDistance, terrainLayerMask);

        Debug.DrawLine(tpc.transform.position + Vector3.up + .5f * tpc.transform.forward, tpc.transform.position + Vector3.up + .5f * tpc.transform.forward + Vector3.down * rayDistance);
        Debug.DrawLine(tpc.transform.position + Vector3.up - .5f * tpc.transform.forward, tpc.transform.position + Vector3.up - .5f * tpc.transform.forward + Vector3.down * rayDistance);
        Debug.DrawLine(tpc.transform.position + Vector3.up, transform.position + Vector3.up + Vector3.down * rayDistance);



        // Terrain height calculations
        if (midRayDown || frontRayDown || backRayDown)
        {
            if(midRayDown)
                newHeight = hit.point.y + 0.1f;
        }
        else
        {
            float gravity = 4.5f;
            falling = true;
            newHeight -= gravity * Time.deltaTime;
        }

        int environmentLayerMask = 1 << 7;
        float environmentRayDistance = 0.5f;
        bool frontRay = Physics.Raycast(tpc.transform.position + Vector3.up, (vInput * m_CharForward + hInput * m_CharRight).normalized, out hit, environmentRayDistance, environmentLayerMask);
        Debug.DrawLine(tpc.transform.position + Vector3.up, tpc.transform.position + Vector3.up + (vInput * m_CharForward + hInput * m_CharRight).normalized * environmentRayDistance, Color.red);
        if (frontRay)
        {
            move = Vector3.zero;
        }
        else { 
            // Move the player (movement will be slightly different depending on the camera type)
            float w_speed = 0;
            if (_cm.type == ThirdPersonCameraMovement.CAMERA_TYPE.FREE_LOOK)
            {
                this.hInput = hInput;
                this.vInput = vInput;
                w_speed = GetPlayerMovementSpeed(run);
                move = (!falling) ? (vInput * m_CharForward + hInput * m_CharRight).normalized * w_speed : Vector3.zero;
                _cam.transform.position += move * Time.deltaTime;

                // Rotate body
                if (!IsLocked())
                    tpc.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(tpc.transform.forward, move, rotationSpeed, 0.0f));
                else 
                { 
                    tpc.transform.LookAt(_lockedEnemy.transform.position);
                    tpc.transform.eulerAngles = Vector3.Scale(tpc.transform.eulerAngles, new Vector3(0, 1, 0));
                }
            }
            else if (_cm.type == ThirdPersonCameraMovement.CAMERA_TYPE.LOCKED)
            {
                w_speed = (vInput > 0) ? walkSpeed : backwards_walk_speed;
                move = (vInput * m_CharForward + hInput * m_CharRight).normalized * ((hInput != 0) ? strafe_speed : w_speed);

                Vector3 camEuler = _cam.transform.eulerAngles;
                tpc.transform.eulerAngles = Vector3.Scale(_cam.transform.eulerAngles, Vector3.up);

            }
        }
        transform.position += move * Time.deltaTime;    // Move the actual player
        transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);
        if (run) _performance.PlayerRunningTick();
        else _performance.PlayerNotRunningTick();
    }

    public bool IsLocked() { return !ReferenceEquals(_lockedEnemy, null); }

    public void LockEnemy(Enemy enemy)
    {
        _lockedEnemy = enemy;
    }

    public void UnlockEnemy()
    {
        _lockedEnemy = null;
    }

    public void MovePlayerRolling(Vector3 rollDirection)
    {
        AnimatorStateInfo rollClipState = tpc.GetFullBodyAnimator().GetCurrentAnimatorStateInfo(0);
        float t = rollClipState.normalizedTime / rollClipState.length;
        float rollMovementMultiplier = _performance.EvaluateRollCurve(t);
        Vector3 rollMove = rollDirection.normalized * rollMovementMultiplier * _rollMultiplierBase;
        transform.position += rollMove * Time.deltaTime;
    }

    public Vector3 GetBodyForward() { return tpc.GetFullBodyAnimator().transform.forward; }

    public Vector3 GetBodyRight() { return tpc.GetFullBodyAnimator().transform.right; }

    private float GetPlayerMovementSpeed(bool run) {
        float result = walkSpeed;

        if (run)
            result *= runSpeedMultiplier;

        result = GetPlayerMovementSpeedAfterEquipLoad(result);

        return result;
    }

    private float GetPlayerMovementSpeedAfterEquipLoad(float speed)
    {
        float result = speed;
        float equipLoadPercentage = _equipment.EquipLoad / _attributes.EquipLoadMax;
        if (equipLoadPercentage == 0) return result;
        else if (equipLoadPercentage < 0.083f) result *= 0.9f;
        else if (equipLoadPercentage < 0.166f) result *= 0.85f;
        else if (equipLoadPercentage < 0.25f) result *= 0.8f;
        else if (equipLoadPercentage < 0.333f) result *= 0.75f;
        else if (equipLoadPercentage < 0.416f) result *= 0.7f;
        else if (equipLoadPercentage < 0.5f) result *= 0.6f;
        else if (equipLoadPercentage < 1f) result *= 0.4f;
        else result *= 0.2f;

        return result;
    }

    public void EnableMovement() { movementEnabled = true; }

    public void DisableMovement() { movementEnabled = false; }


    public void Restart()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        _performance.Restart();

        alive = true;

        _playerState = new PlayerSittingState();
        _playerState.Enter(this);
    }

    public void UseCurrentItem()
    { 
        
    }

    #region Animations

    public void AnimatePlayer(bool run)
    {
        Animator anim = tpc.GetFullBodyAnimator();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("FreeLookBlendTreeJog") ||
            anim.GetCurrentAnimatorStateInfo(0).IsName("FreeLookBlendTree"))
        {
            SelectCorrectAnimator(run);
            UpdateFreeLookBlend();
            tpc.GetFullBodyAnimator().SetFloat("Blend", animFreeLookBlend);
        }
        else if(anim.GetCurrentAnimatorStateInfo(0).IsName("LockViewBlendTreeJog"))
        {
            SelectCorrectAnimator(run);
            UpdateLockViewBlend();
            tpc.GetFullBodyAnimator().SetFloat("HInput", animLockViewBlendX);
            tpc.GetFullBodyAnimator().SetFloat("VInput", animLockViewBlendY);
        }
    }

    private void SelectCorrectAnimator(bool run)
    {
        Animator anim = tpc.GetFullBodyAnimator();
        if(IsLocked() && !anim.GetCurrentAnimatorStateInfo(0).IsName("LockViewBlendTreeJog")) tpc.GetFullBodyAnimator().Play("LockViewBlendTreeJog");
        else if (!IsLocked() && run && !anim.GetCurrentAnimatorStateInfo(0).IsName("FreeLookBlendTreeJog")) tpc.GetFullBodyAnimator().Play("FreeLookBlendTreeJog");
        else if (!IsLocked() && !run && !anim.GetCurrentAnimatorStateInfo(0).IsName("FreeLookBlendTree")) tpc.GetFullBodyAnimator().Play("FreeLookBlendTree");
    }

    private void UpdateFreeLookBlend()
    {
        float movement = Mathf.Clamp01(move.magnitude);
        float t = 2.5f;

        int blendDirection = GetBlendDirection(animFreeLookBlend, movement);
        animFreeLookBlend = Mathf.Clamp01(animFreeLookBlend + t * blendDirection * Time.deltaTime);

    }

    private void UpdateLockViewBlend()
    {
        float t = 2f;
        int blendDirectionX = GetBlendDirection(animLockViewBlendX, Mathf.Clamp(hInput, -1, 1));
        int blendDirectionY = GetBlendDirection(animLockViewBlendY, Mathf.Clamp(vInput, -1, 1));

        animLockViewBlendX = Mathf.Clamp(animLockViewBlendX + t * blendDirectionX * Time.deltaTime, -1, 1);
        animLockViewBlendY = Mathf.Clamp(animLockViewBlendY + t * blendDirectionY * Time.deltaTime, -1, 1);
    }

    private int GetBlendDirection(float animValue, float inputValue)
    {
        if (animValue > inputValue) return -1;
        else if (animValue == inputValue) return 0;
        else return 1;
    }

    public void PlayAnimation(string animationName)
    {
        tpc.GetFullBodyAnimator().Play(animationName);
    }

    public bool HasFinishedAnimation(string animationName)
    {
        return !tpc.GetFullBodyAnimator().GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    #endregion

    #region Input buttons
    private void OnInputEvent(NewPlayerInput input)
    {
        if (!GameManager.s.IsInState(GAME_STATE.GAMEPLAY))
            return;

        if (movementEnabled) {
            _lastInput = input;
            PlayerState newState = _playerState.HandleInput(this, ref input);

            if (!ReferenceEquals(newState, null))
            {
                _playerState = newState;
                _playerState.Enter(this);
            }
        }
    }
    #endregion
    
    #region GameCharacter Inherited Methods
    public override void OnReceiveHit()
    {
        Debug.LogWarning("Player was hit!");
        float previousHealth = _performance.HP;
        _performance.DealDamage();
        EventsManager.instance.NotifyEvent(PLAYER_EVENTS.PLAYER_HEALTH_CHANGE, _performance.HP, previousHealth, _attributes.VitalityMax);
        if (_performance.HP > 0)
            _playerState = new PlayerHitState();
        else 
        {
            alive = false;
            _playerState = new PlayerDeathState();
            EventsManager.instance.NotifyEvent(GAME_EVENTS.PLAYER_DEAD);
        }
        _playerState.Enter(this);
    }

    public override void EnableCurrentWeapons()
    {
        PlayerWeapon[] activeWeapons = weaponsParent.GetComponentsInChildren<PlayerWeapon>();
        foreach (PlayerWeapon weapon in activeWeapons)
        {
            BoxCollider collider = weapon.GetComponent<BoxCollider>();
            collider.enabled = true;
        }
    }

    public override void DisableCurrentWeapons()
    {
        PlayerWeapon[] activeWeapons = weaponsParent.GetComponentsInChildren<PlayerWeapon>();
        foreach (PlayerWeapon weapon in activeWeapons)
        {
            BoxCollider collider = weapon.GetComponent<BoxCollider>();
            collider.enabled = false;
        }
    }

    public override void OnCharacterSpawn()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        _playerState = new PlayerSittingState();
        _playerState.Enter(this);
    }

    #endregion

}
