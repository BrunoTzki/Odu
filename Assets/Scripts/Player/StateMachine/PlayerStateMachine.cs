using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    #region Exposed Variables
    [Header("Movement")]
    [Tooltip("Move speed of the character in m/s")]
    [SerializeField] private float _moveSpeed = 5.335f;
    public float MoveSpeed {get { return _moveSpeed; }}

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    [SerializeField] private float _rotationSmoothTime = 0.12f;
    public float RotationSmoothTime { get { return _rotationSmoothTime;}}

    [Tooltip("Acceleration and deceleration")]
    [SerializeField] private float _speedChangeRate = 10.0f;
    public float SpeedChangeRate { get { return _speedChangeRate;}}

    [SerializeField] private AudioClip _landingAudioClip;
    [SerializeField] private AudioClip[] _footstepAudioClips;
    [Range(0, 1)] 
    [SerializeField] private float _footstepAudioVolume = 0.5f;


    [Space(10)]
    [Header("Jump")]
    [Tooltip("The height the player can jump")]
    [SerializeField] private float _jumpHeight = 1.2f;
    public float JumpHeight {get { return _jumpHeight; }}

    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    [SerializeField] private float _jumpTimeout = 0.50f;
    public float JumpTimeout {get { return _jumpTimeout; }}


    [Space(10)]
    [Header("Falling")]
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    [SerializeField] private float _gravity = -15.0f;
    public float Gravity {get { return _gravity; }}

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    [SerializeField] private float _fallTimeout = 0.15f;
    public float FallTimeout {get { return _fallTimeout; }}


    [Space(10)]
    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    [SerializeField] private bool _grounded = true;
    public bool Grounded {get { return _grounded; }}

    [Tooltip("Useful for rough ground")]
    [SerializeField] private float _groundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    [SerializeField] private float _groundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    [SerializeField] private LayerMask _groundLayers;


    [Space(10)]
    [Header("Dashing")]
    [Tooltip("Speed the player has while dashing")]
    [SerializeField] private float _dashSpeed = 20f;
    public float DashSpeed {get { return _dashSpeed;}}

    [Tooltip("How long the dash will last")]
    [SerializeField] private float _dashTime = 0.25f;
    public float DashTime {get { return _dashTime;}}

    [Tooltip("How long the player has to wait before they can dash again")]
    [SerializeField] private float _dashTimeout = 0.5f;
    public float DashTimeout {get { return _dashTimeout;}}


    [Space(10)]
    [Header("Combat")]
    [Tooltip("The current active combat tool the player is using")]
    [SerializeField] private BaseToolSO _currentTool;
    public BaseToolSO CurrentTool {get { return _currentTool; }}

    [Tooltip("How long to wait for the combo to end after the last attack")]
    [SerializeField] private float _comboTimerDelay = 1f;
    public float ComboTimerDelay {get { return _comboTimerDelay; }}

    [Tooltip("How long to wait before a new combo can be started")]
    [SerializeField] private float _comboWaitTime = 0.5f;
    public float ComboWaitTime {get { return _comboWaitTime; }}

    [Tooltip("The percentage which the attack animation will be considered close to finished")]
    [SerializeField, Range(0.5f, 0.99f)] private float _animEndPct = 0.9f;
    public float AnimEndPct {get { return _animEndPct; }}

    [Tooltip("Gameobject to attach the weapon")]
    [SerializeField] private Transform _weaponHolder;

    [Tooltip("The distance the player can reach the enemy")]
    [SerializeField] private float _reachDistance = 3f;
    public float ReachDistance { get { return _reachDistance; }}

    [Tooltip("Time it takes for the player to reach the enemy")]
    [SerializeField] private float _reachDuration = .5f;
    public float ReachDuration { get { return _reachDuration; }}

    #endregion

    #region Movement Variables
    private float _speed;
    public float Speed {get { return _speed;} set { _speed = value; } }
    private float _animationBlend;
    public float AnimationBlend {get { return _animationBlend;} set { _animationBlend = value; } }//not used
    private float _targetRotation = 0.0f;
    public float TargetRotation {get { return _targetRotation;} set { _targetRotation = value; } }
    private float _rotationVelocity;
    private float _verticalVelocity;
    public float VerticalVelocity {get {return _verticalVelocity;} set {_verticalVelocity = value;}}
    private float _terminalVelocity = 53.0f;
    public float TerminalVelocity {get { return _terminalVelocity; }}
    private Vector3 _targetDirection;
    public Vector3 TargetDirection {get { return _targetDirection;} set { _targetDirection = value;}}
    private float _targetSpeed;
    public float TargetSpeed {get { return _targetSpeed; } set {_targetSpeed = value; }}

    #endregion

    #region External Components
    private CharacterController _controller;
    public CharacterController Controller {get {return _controller;}}
    private Transform _mainCamera;
    public Transform MainCamera {get {return _mainCamera;}}
    private Animator _animator;
    public Animator Animator {get { return _animator; }}

    #endregion

    #region Combat Variables
    private float _lastComboEnd;
    public float LastComboEnd {get {return _lastComboEnd; } set {_lastComboEnd = value;}}
    private int _comboCounter;
    public int ComboCounter {get {return _comboCounter; } set {_comboCounter = value;}}
    private bool _comboTimerRunning = false;
    public bool ComboTimerRunning {get {return _comboTimerRunning;} set {_comboTimerRunning = value;}}
    private Weapon _currentWeapon;
    public Weapon CurrentWeapon {get { return _currentWeapon; }}

    #endregion

    #region Timeout deltatime
    // Jump
    private float _jumpTimeoutDelta;
    public float JumpTimeoutDelta {get { return _jumpTimeoutDelta; } set {_jumpTimeoutDelta = value; }}
    // Falling
    private float _fallTimeoutDelta;
    public float FallTimeoutDelta {get { return _fallTimeoutDelta; } set {_fallTimeoutDelta = value; }}
    // Dash
    private float _dashTimeoutDelta;
    public float DashTimeoutDelta {get { return _dashTimeoutDelta; } set {_dashTimeoutDelta = value;}}
    // Combat
    private float _comboTimeoutDelta;
    public float ComboTimeoutDelta {get {return _comboTimeoutDelta;} set {_comboTimeoutDelta = value;}}

    #endregion

    #region AnimationIDs
    private bool _hasAnimator;
    public bool HasAnimator {get { return _hasAnimator; }}

    private int _animIDSpeed;
    public int AnimIDSpeed {get { return _animIDSpeed; }}
    private int _animIDGrounded;
    private int _animIDJump;
    public int AnimIDJump {get { return _animIDJump; }}
    private int _animIDFreeFall;
    public int AnimIDFreeFall {get { return _animIDFreeFall; }}
    private int _animIDMotionSpeed;
    private int _animIDDash;
    public int AnimIDDash {get { return _animIDDash; }}
    private int _animIDAttack;
    public int AnimIDAttack {get { return _animIDAttack;}}

    #endregion

    #region State Variables
    //state variables
    private PlayerBaseState _currentState;
    public PlayerBaseState CurrentState {get { return _currentState; } set { _currentState = value; }}
    private PlayerStateFactory _states;

    #endregion

    private void Awake()
    {
        //setup state
        _states = new PlayerStateFactory(this);

        _mainCamera = Camera.main.transform;

        _currentWeapon = Instantiate(_currentTool.ToolWeapon, _weaponHolder).GetComponent<Weapon>();
        _currentWeapon.Deactivate();
    }

    private void Start()
    {
        _currentState = _states.Grounded();
        _currentState.EnterState();
        
        _hasAnimator = TryGetComponent(out _animator);
        _controller = GetComponent<CharacterController>();

        AssignAnimationIDs();

        // reset our timeouts on start
        _jumpTimeoutDelta = _jumpTimeout;
        _fallTimeoutDelta = _fallTimeout;
        _dashTimeoutDelta = _dashTimeout;
        _comboTimeoutDelta = _comboTimerDelay;

        _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
    }

    private void Update() {            
        //Debug.Log(_currentState.GetActiveStates(), this);
        /*
        if(_animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack")){
            Debug.Log(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime, this);
        }*/

        //Debug.Log("Attack input: " + GameInput.Instance.IsAttacking(), this);
        //Debug.Log("Dash input: " + GameInput.Instance.IsDashing(), this);

        HandleGravity();
        GroundedCheck();
        
        HandleTimeouts();


        //Vector3 inputDirection = new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized;
        //_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;

        //_targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _currentState.UpdateStates();



        // move the player
        _controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        //HandleMove();
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDDash = Animator.StringToHash("Dash");
        _animIDAttack = Animator.StringToHash("Attack");
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
        _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        //Debug.Log(_grounded, this);

        // update animator if using character
        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, _grounded);
        }
    }

    private void HandleGravity(){
        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
    }

    private void HandleTimeouts(){
        //dash
        if (_dashTimeoutDelta >= 0.0f){
            _dashTimeoutDelta -= Time.deltaTime;
        }

        //combat
        if(_comboTimerRunning && _comboTimeoutDelta > 0f){
            _comboTimeoutDelta -= Time.deltaTime;
            if(_comboTimeoutDelta <= 0f){
                _comboTimerRunning = false;
                _comboCounter = 0;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_grounded) Gizmos.color = Color.green;
        else Gizmos.color = Color.red;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundedRadius);

        Gizmos.color = Color.blue;
        //Draw Target Direction
        Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z), _targetDirection.normalized);


        if(GameInput.Instance.GetMove() == Vector2.zero){
            Gizmos.DrawWireSphere(transform.position,_reachDistance);
        } else {
            Gizmos.matrix = transform.localToWorldMatrix;
            float halfDistance = _reachDistance * 0.5f;
            Gizmos.DrawWireCube(Vector3.zero + Vector3.forward * halfDistance, Vector3.one * halfDistance);
        }


        //Vector3 inputDirection = new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized;
        //Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (_footstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, _footstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(_footstepAudioClips[index], transform.TransformPoint(_controller.center), _footstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(_landingAudioClip, transform.TransformPoint(_controller.center), _footstepAudioVolume);
        }
    }
}