using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(WeaponManager), typeof(Health))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    PlayerRuntimeSet _playerRuntimeSet = null;

    [Header("Child Entity References")]
    [SerializeField]
    Transform _cameraFollowTarget = null;
    [SerializeField]
    Camera _playerCamera = null;

    [Header("Movement Settings")]
    [SerializeField]
    float _moveSpeed = 1.0F;
    [SerializeField]
    float _characterMass = 1.0F;
    [SerializeField]
    float _jumpStrength = 1.0F;
    [SerializeField, Range(0.0F, 1.0F)]
    float _aimingLookSpeedMultiplier = 0.5F;
    [SerializeField]
    float _aimTransitionSpeed = 1.0F;
    [SerializeField, Range(20.0F, 120.0F)]
    float _defaultFOV = 60.0F;
    [SerializeField, Range(20.0F, 120.0F)]
    float _aimingFOV = 40.0F;

    [Header("Input Settings")]
    [SerializeField]
    PlayerInputSettings _inputSettings = null;

    const float GRAVITY_ACCELERATION = 9.81F;
    const float GROUND_CHECK_EPSILON = 0.1F;

    PlayerInput _playerInput = null;
    CharacterController _characterController = null;
    WeaponManager _weaponManager = null;
    Health _characterHealth = null;
    Vector2 _rawMoveInput = Vector2.zero;
    Vector2 _rawLookInput = Vector2.zero;

    bool _isGrounded = false;
    Vector3 _gravityVector = Vector3.zero;
    bool _justJumped = false;
    bool _isAiming = false;
    bool _isShooting = false;
    bool _stoppedShooting = false;

    void OnValidate()
    {
        if (_playerCamera != null)
        {
            _playerCamera.fieldOfView = _defaultFOV;
        }
    }

    void Start()
    {
        // Lock player cursor to game window
        Cursor.lockState = CursorLockMode.Locked;
        PauseManager.Instance.UIManager.OnUnpause += OnUnpause;

        _playerInput = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();
        _weaponManager = GetComponent<WeaponManager>();
        _characterHealth = GetComponent<Health>();
        _characterHealth.OnHealthDepleted += OnHealthDepleted;
        GroundCheck();

        // Add player to runtime set
        _playerRuntimeSet.Add(this);
    }

    void OnDestroy()
    {
        _playerRuntimeSet.Remove(this);
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Check character groundedness
        bool wasGrounded = _isGrounded;
        GroundCheck();
        if (wasGrounded != _isGrounded)
        {
            Debug.Log($"Grounded {wasGrounded} -> {_isGrounded}");
        }

        // Apply look rotation to character
        float aimSpeedMultiplier = _isAiming ? _aimingLookSpeedMultiplier : 1.0F;
        float yLookMultiplier = _inputSettings.invertYLook ? -1.0F : 1.0F;
        _cameraFollowTarget.Rotate(Vector3.right, yLookMultiplier * _inputSettings.lookSensitivity * aimSpeedMultiplier * _rawLookInput.y);
        transform.Rotate(Vector3.up, _inputSettings.lookSensitivity * aimSpeedMultiplier * _rawLookInput.x);

        Vector3 euler = _cameraFollowTarget.localEulerAngles;
        euler.z = 0;
        euler.y = 0;
        euler.x = euler.x < 180.0F ? Mathf.Clamp(euler.x, 0, 80) : Mathf.Clamp(euler.x, 280, 360);
        _cameraFollowTarget.localEulerAngles = euler;

        // Move character
        Vector3 moveDirection = transform.forward * _rawMoveInput.y + transform.right * _rawMoveInput.x;
        Vector3 moveVector = moveDirection * _moveSpeed * Time.deltaTime;
        _characterController.Move(moveVector);

        // Apply jump
        if (_justJumped && _isGrounded)
        {
            _isGrounded = false;
            _gravityVector = Vector3.up * _jumpStrength;
            _characterController.Move(Vector3.up * 2.0F * GROUND_CHECK_EPSILON);
        }
        _justJumped = false;

        // Apply aiming FOV
        if (_isAiming)
        {
            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, _aimingFOV, _aimTransitionSpeed * Time.deltaTime);
        }
        else
        {
            _playerCamera.fieldOfView = Mathf.Lerp(_playerCamera.fieldOfView, _defaultFOV, _aimTransitionSpeed * Time.deltaTime);
        }

        // Shoot weapon
        if (_isShooting)
        {
            _weaponManager.Shoot();
        }
        
        // Mark active weapon as ready when player stops shooting
        if (_stoppedShooting)
        {
            _weaponManager.SetWeaponReady();
        }
        _stoppedShooting = false;

        // Apply gravity
        if (!_isGrounded)
        {
            _gravityVector += Vector3.down * _characterMass * GRAVITY_ACCELERATION * Time.deltaTime;
            _characterController.Move(_gravityVector * Time.deltaTime);
        }
    }

    private void GroundCheck()
    {
        _isGrounded = false;
        float checkRadius = _characterController.radius;
        float checkDistance = GROUND_CHECK_EPSILON + 0.5F * _characterController.height;
        checkDistance -= checkRadius;

        int mask = LayerMask.GetMask("Ignore Raycast");
        mask = ~mask; // We want everything BUT the ignore raycast layer
        if (Physics.SphereCast(transform.position, checkRadius, Vector3.down, out RaycastHit _, checkDistance, mask))
        {
            _isGrounded = true;
            _gravityVector = Vector3.zero;
        }
    }

    private void OnHealthDepleted()
    {
        Debug.Log("Owno, the player died :(");
    }

    public void OnMove(InputValue value)
    {
        _rawMoveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        _rawLookInput = value.Get<Vector2>();
    }

    public void OnJump()
    {
        _justJumped = true;
    }

    public void OnReload()
    {
        _weaponManager.Reload();
    }

    public void OnCycleWeapon(InputValue value)
    {
        if (value.Get<float>() > 0.0F)
        {
            _weaponManager.SelectNextWeapon();
        }
        
        if (value.Get<float>() < 0.0F)
        {
            _weaponManager.SelectPreviousWeapon();
        }
    }

    public void OnAim(InputValue value)
    {
        _isAiming = value.Get<float>() > 0.5F;
    }

    public void OnShoot(InputValue value)
    {
        _isShooting = value.Get<float>() > 0.5F;
        if (!_isShooting)
        {
            _stoppedShooting = true;
        }
    }

    public void OnPause()
    {
        if (!PauseManager.Instance.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            PauseManager.Instance.PauseGame();
            _playerInput.SwitchCurrentActionMap("Paused");
        }
    }

    public void OnUnpause()
    {
        if (PauseManager.Instance.IsPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            PauseManager.Instance.UnpauseGame();
            _playerInput.SwitchCurrentActionMap("Gameplay");
        }
    }
}
