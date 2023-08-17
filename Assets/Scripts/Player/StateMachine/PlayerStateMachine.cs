using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
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

        [SerializeField] private AudioClip LandingAudioClip;
        [SerializeField] private AudioClip[] FootstepAudioClips;
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
        [SerializeField] private LayerMask GroundLayers;


        [Space(10)]
        [Header("Dashing")]
        [SerializeField] private float _dashSpeed = 20f;
        public float DashSpeed {get { return _dashSpeed;}}

        [SerializeField] private float _dashTime = 0.25f;
        public float DashTime {get { return _dashTime;}}

        [SerializeField] private float _dashTimeout = 0.5f;
        public float DashTimeout {get { return _dashTimeout;}}


        [Space(10)]
        [Header("Combat")]
        [SerializeField] private BaseToolSO _currentTool;



        // player
        private float _speed;
        public float Speed {get { return _speed;} set { _speed = value; } }
        private float _animationBlend;
        public float AnimationBlend {get { return _animationBlend;} set { _animationBlend = value; } }
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

        private CharacterController _controller;
        public CharacterController Controller {get {return _controller;}}
        private Transform _mainCamera;
        public Transform MainCamera {get {return _mainCamera;}}

        // timeout deltatime
        private float _jumpTimeoutDelta;
        public float JumpTimeoutDelta {get { return _jumpTimeoutDelta; } set {_jumpTimeoutDelta = value; }}
        private float _fallTimeoutDelta;
        public float FallTimeoutDelta {get { return _fallTimeoutDelta; } set {_fallTimeoutDelta = value; }}
        private float _dashTimeoutDelta;
        public float DashTimeoutDelta {get { return _dashTimeoutDelta; } set {_dashTimeoutDelta = value;}}

        // animation IDs
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

        private Animator _animator;
        public Animator Animator {get { return _animator; }}

        private bool _hasAnimator;
        public bool HasAnimator {get { return _hasAnimator; }}

        //state variables
        private PlayerBaseState _currentState;
        public PlayerBaseState CurrentState {get { return _currentState; } set { _currentState = value; }}
        private PlayerStateFactory _states;
    
    private void Awake()
        {
            //setup state
            _states = new PlayerStateFactory(this);

            _mainCamera = Camera.main.transform;
        }

        private void Start()
        {
            _currentState = _states.Grounded();
            _currentState.EnterState();
            
            //_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            //_input = GetComponent<StarterAssetsInputs>();

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = _jumpTimeout;
            _fallTimeoutDelta = _fallTimeout;
            _dashTimeoutDelta = _dashTimeout;

            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        }

        private void Update() {            
            _currentState.PrintActiveStates();

            HandleGravity();
            GroundedCheck();
            
            HandleTimeouts();

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
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

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
            if (_dashTimeoutDelta >= 0.0f){
                _dashTimeoutDelta -= Time.deltaTime;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundedRadius);

            //Draw Target Direction
            Gizmos.DrawRay(new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), _targetDirection.normalized);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), _footstepAudioVolume);
                }
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), _footstepAudioVolume);
            }
        }
}
