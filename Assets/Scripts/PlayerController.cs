using Cinemachine;
using System.Collections;
using System.Threading;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
#endif

public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public float ClimbingSpeed = 1.0f;
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.335f;

    [Space(10)]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float SpeedChangeRate = 10.0f;
    public float baseSensitivity = 0.12f;
    public float LookSensitivity = 1.0f;

    [Space(10)]
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;

    [Space(10)]
    public float JumpTimeout = 0.5f;
    public float FallTimeout = 0.15f;

    [Header("Player Grounded")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;
    public LayerMask GroundLayers;

    [Header("Player Climb")]
    public bool Cliff = false;
    public float CliffCheckOffsetY = 0.09f;
    public float CliffCheckOffsetZ = 0.09f;
    public float CliffCheckRadius = 0.18f;
    private bool _isClimbingUp = false;
    public float ClimbingFinishedHeight = 0.5f;
    public LayerMask CliffLayers;

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;
    private Cinemachine3rdPersonFollow _cinemachineTransposer;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;

    //cinemachine
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private static float _cinemachineTargetYaw;
    private static float _cinemachineTargetPitch;
    private float aimFOV = 20f;
    private float minFOV = 40f;
    private float maxFOV = 60f;
    private Vector3 aimFollowOffset = new Vector3(2f, 0, 1f);
    private Vector3 normalFollowOffset = new Vector3(1f, 0, 0);
    public float AimVerticalSensitivity = 0.5f;

    //player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;
    private bool _isClimbing = false;
    public static bool _isGliding = false;
    private bool _isAiming = false;
    private bool _aimPressedLastFrame = false;

    //timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    //animation IDs
    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;
    private int _animIDCliffCheck;

#if ENABLE_INPUT_SYSTEM
    private PlayerInput _playerInput;
#endif
    public Animator _wingAnimator;
    private Animator _animator;
    private CharacterController _controller;
    private PlayerInputHandler _input;
    private GameObject _mainCamera;

    private const float _threshold = 0.01f;

    private bool _hasAnimator;
    private bool rotateOnMove = true;

    public CharacterData characterData;

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }

        _animator = GetComponent<Animator>();
        _hasAnimator = TryGetComponent(out _animator);

        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        
        _controller = transform.parent.GetComponent<CharacterController>();
        _input = transform.parent.GetComponent<PlayerInputHandler>();
#if ENABLE_INPUT_SYSTEM
        _playerInput = GetComponent<PlayerInput>();
#endif

        AssignAnimationIDs();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

        CinemachineCameraTarget = transform.parent.GetChild(0).gameObject;

        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        _hasAnimator = TryGetComponent(out _animator);

        if (_input.aim && !_aimPressedLastFrame)
        {
            _isAiming = !_isAiming;

            if (_isAiming)
            {
                RotateCameraTowardsMouse();
                AlignPlayerWithCamera();
                EnterAimMode();
            }
            else
            {
                ExitAimMode();
            }
        }

        _aimPressedLastFrame = _input.aim;

        _animator.SetBool("isAiming", _isAiming);

        if (_isAiming)
        {
            AimMove();
            UpdateCameraRotationWhileAiming();
        }
        else
        {
            JumpAndGravity();
            GroundedCheck();
            CliffCheck();
            Move();
            Climb();
        }
    }

    private void LateUpdate()
    {
        if (!_isAiming)
        {
            CameraRotation();
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("MoveSpeed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDCliffCheck = Animator.StringToHash("Cliff");
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDGrounded, Grounded);
        }
    }

    private void CliffCheck()
    {
        Vector3 spherePosiiton = new Vector3(transform.position.x, transform.position.y + CliffCheckOffsetY,
            transform.position.z + CliffCheckOffsetZ);

        Cliff = Physics.CheckSphere(spherePosiiton, CliffCheckRadius, CliffLayers,
            QueryTriggerInteraction.Ignore);

        if (_hasAnimator)
        {
            _animator.SetBool(_animIDCliffCheck, Cliff);

            if (Cliff)
            {
                Grounded = false;
                transform.rotation = Quaternion.identity;
                _animator.SetBool("Climb", true);
                _animator.SetBool(_animIDGrounded, false);
                _animator.SetBool(_animIDFreeFall, false);
            }
        }

        if(!Cliff && _isClimbing)
        {
            StartClimbUpAnimation();
        }
    }

    private void CameraRotation()
    {
        if (_animator.GetBool("Attacking") || _isAiming) return;

        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            _cinemachineTargetYaw += _input.look.x * baseSensitivity * LookSensitivity;
            _cinemachineTargetPitch += _input.look.y * baseSensitivity * LookSensitivity;
        }
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);

        if (_input.zoom != 0)
        {
            float newFOV = virtualCamera.m_Lens.FieldOfView - (_input.zoom / 120f);
            virtualCamera.m_Lens.FieldOfView = Mathf.Clamp(newFOV, minFOV, maxFOV);
            _input.zoom = 0;
        }
    }

    private void RotateCameraTowardsMouse()
    {
        if (_input.look.sqrMagnitude >= _threshold)
        {
            _cinemachineTargetYaw += _input.look.x * baseSensitivity * LookSensitivity;
            _cinemachineTargetPitch += _input.look.y * baseSensitivity * LookSensitivity;
        }
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    private void AlignPlayerWithCamera()
    {
        _targetRotation = _cinemachineTargetYaw;
        transform.rotation = Quaternion.Euler(0.0f, _targetRotation, 0.0f);
    }

    private void UpdateCameraRotationWhileAiming()
    {
        Vector2 mouseDelta = _input.look;

        _cinemachineTargetYaw += mouseDelta.x * baseSensitivity * LookSensitivity;
        _cinemachineTargetPitch -= mouseDelta.y * baseSensitivity * LookSensitivity * AimVerticalSensitivity;

        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);


        transform.rotation = Quaternion.Euler(0.0f, _cinemachineTargetYaw, 0.0f);
    }

    private void EnterAimMode()
    {
        _animator.SetBool("Attaking", false);
        virtualCamera.m_Lens.FieldOfView = aimFOV;
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = aimFollowOffset;

        Cursor.lockState = CursorLockMode.Locked;

        UIManager.Instance.SetCrosshairActive(true);
    }

    private void ExitAimMode()
    {
        _animator.SetBool("Attaking", false);
        virtualCamera.m_Lens.FieldOfView = maxFOV;
        virtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>().ShoulderOffset = normalFollowOffset;

        Cursor.lockState = CursorLockMode.None;

        UIManager.Instance.SetCrosshairActive(false);
    }

    private void Move()
    {
        if (_isClimbing || _isAiming) return;

        float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

        float speedOffset = 0.1f;
        float inputMagnitude = 1.0f;

        if (currentHorizontalSpeed < targetSpeed - speedOffset ||
            currentHorizontalSpeed > targetSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_input.move != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                _mainCamera.transform.eulerAngles.y;

            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);
            if (rotateOnMove) transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }

        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

        _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
            new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        if (_hasAnimator)
        {
            _animator.SetFloat(_animIDSpeed, _animationBlend, 0.05f, Time.deltaTime);
            _animator.SetFloat(_animIDMotionSpeed, inputMagnitude, 0.1f, Time.deltaTime);
        }
    }

    private void AimMove()
    {
        _animator.SetBool("isAiming", _isAiming);

        float targetSpeed = MoveSpeed;

        if (_input.move == Vector2.zero) targetSpeed = 0.0f;

        float verticalInput = _input.move.x;
        float horizontalInput = _input.move.y;

        Vector3 aimMoveDirection = new Vector3(verticalInput, 0.0f, horizontalInput);
        aimMoveDirection = transform.TransformDirection(aimMoveDirection);

        _controller.Move(aimMoveDirection * targetSpeed * Time.deltaTime);

        if (_hasAnimator)
        {
            _animator.SetFloat("horizontal", horizontalInput);
            _animator.SetFloat("vertical", verticalInput);
        }
    }

    private void Climb()
    {
        if (Cliff)
        {
            Grounded = false;
            _isClimbing = true;
            _animator.SetBool("Climb", true);
            _controller.enabled = true;

            _verticalVelocity = 0.0f;

            float verticalInput = _input.move.x;
            float horizontalInput = _input.move.y;

            Vector3 moveDirection = new Vector3(verticalInput, horizontalInput, 0.0f);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.y = Mathf.Clamp(moveDirection.y, -1f, 1f);            
           
            _controller.Move(moveDirection * ClimbingSpeed * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat("horizontal", horizontalInput);
                _animator.SetFloat("vertical", verticalInput);
            }
            
        }
        else
        {
            _isClimbing = false;
            _animator.SetBool("Climb", false);
            _controller.enabled = true;
        }
    }

    private void StartClimbUpAnimation()
    {
        _isClimbing = false;
        _isClimbingUp = true;
        _verticalVelocity = 4.0f;
        _animator.SetBool("Climb", false);
        _animator.SetTrigger("ClimbUp");
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;

            if (_hasAnimator)
            {
                _animator.SetBool(_animIDJump, false);
                _animator.SetBool(_animIDFreeFall, false);
                _animator.SetBool("Glide", false);
            }

            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2.0f;
            }

            if (_input.jump && _jumpTimeoutDelta <= 0.0f)
            {
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, true);
                }
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            if((_verticalVelocity < 0.0f && !_isGliding))
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }

            if(_input.jump && (_verticalVelocity < 0.0f || _input.windfield))
            {
                if (_isGliding)
                    StopGliding();
                else
                    StartGliding();
            }

            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDFreeFall, true);
                }
            }
            _input.jump = false;
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }

        if (_isGliding)
        {
            Gliding();
        }
    }

    private void StartGliding()
    {
        _isGliding = true;
        if(_hasAnimator)
        {
            _animator.SetBool("Glide", true);
            _animator.SetBool(_animIDJump, false);
            _wingAnimator.SetTrigger("Glide");
        }
    }

    private void StopGliding()
    {
        _isGliding = false;
        if (_hasAnimator)
        {
            _animator.SetBool("Glide", false);
            _animator.SetBool(_animIDJump, true);
        }
    }

    private void Gliding()
    {
        _verticalVelocity = -0.5f;
        if(_input.windfield)
        {
            _verticalVelocity = 0.5f;
        }
        Vector3 move = new Vector3(_input.move.x, 0, _input.move.y);

        move = transform.TransformDirection(move);

        _controller.Move(move * MoveSpeed * Time.deltaTime);

        _controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);

        if (Grounded)
        {
            _isGliding = false;
            if (_hasAnimator)
            {
                _animator.SetBool("Glide", false);
            }
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SetSensitivity(float newSensitivity)
    {
        LookSensitivity = newSensitivity;
    }

    public void SetRotateOnMove(bool newRotateOnMove)
    {
        rotateOnMove = newRotateOnMove;
    }
}
