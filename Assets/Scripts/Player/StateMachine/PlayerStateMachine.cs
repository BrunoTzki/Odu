using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        [SerializeField] private float _moveSpeed = 5.335f;
        public float MoveSpeed {get { return _moveSpeed; }}

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        [SerializeField] private float _rotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        [SerializeField] private float _speedChangeRate = 10.0f;

        [SerializeField] private AudioClip LandingAudioClip;
        [SerializeField] private AudioClip[] FootstepAudioClips;
        [Range(0, 1)] 
        [SerializeField] private float _footstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        [SerializeField] private float _jumpHeight = 1.2f;
        public float JumpHeight {get { return _jumpHeight; }}

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        [SerializeField] private float _gravity = -15.0f;
        public float Gravity {get { return _gravity; }}

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        [SerializeField] private float _jumpTimeout = 0.50f;
        public float JumpTimeout {get { return _jumpTimeout; }}

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        [SerializeField] private float _fallTimeout = 0.15f;
        public float FallTimeout {get { return _fallTimeout; }}

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


        // player
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        public float VerticalVelocity {get {return _verticalVelocity;} set {_verticalVelocity = value;}}
        private float _terminalVelocity = 53.0f;
        public float TerminalVelocity {get { return _terminalVelocity; }}
        private Vector3 _targetDirection;
        private float _targetSpeed;
        public float TargetSpeed {get { return _targetSpeed; } set {_targetSpeed = value; }}

        // timeout deltatime
        private float _jumpTimeoutDelta;
        public float JumpTimeoutDelta {get { return _jumpTimeoutDelta; } set {_jumpTimeoutDelta = value; }}
        private float _fallTimeoutDelta;
        public float FallTimeoutDelta {get { return _fallTimeoutDelta; } set {_fallTimeoutDelta = value; }}

        // animation IDs
        private int _animIDSpeed;
        public int AnimIDSpeed {get { return _animIDSpeed; }}
        private int _animIDGrounded;
        private int _animIDJump;
        public int AnimIDJump {get { return _animIDJump; }}
        private int _animIDFreeFall;
        public int AnimIDFreeFall {get { return _animIDFreeFall; }}
        private int _animIDMotionSpeed;

        private Animator _animator;
        public Animator Animator {get { return _animator; }}
        private CharacterController _controller;
        private Transform _mainCamera;

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

            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        }

        private void Update() {
            Debug.Log(_currentState.ToString());
            HandleGravity();
            GroundedCheck();

            _currentState.UpdateStates();

            // move the player
            //_controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
            HandleMove();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _groundedOffset,
                transform.position.z);
            _grounded = Physics.CheckSphere(spherePosition, _groundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

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

        private void HandleMove(){

            // a reference to the players current horizontal velocity
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = GameInput.Instance.GetMove().magnitude;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < _targetSpeed - speedOffset || currentHorizontalSpeed > _targetSpeed + speedOffset)
            {
                // creates curved result rather than a linear one giving a more organic speed change
                // note T in Lerp is clamped, so we don't need to clamp our speed
                _speed = Mathf.Lerp(currentHorizontalSpeed, _targetSpeed * inputMagnitude, Time.deltaTime * _speedChangeRate);

                // round speed to 3 decimal places
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = _targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, _targetSpeed, Time.deltaTime * _speedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            // normalise input direction
            Vector3 inputDirection = new Vector3(GameInput.Instance.GetMove().x, 0.0f, GameInput.Instance.GetMove().y).normalized;

            // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
            // if there is a move input rotate player when the player is moving
            if (GameInput.Instance.GetMove() != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, _rotationSmoothTime);

                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // move the player
            _controller.Move(_targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _speed);
                //_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (_grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - _groundedOffset, transform.position.z), _groundedRadius);
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
