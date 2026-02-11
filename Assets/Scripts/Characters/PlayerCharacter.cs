using UnityEngine;
using UnityEngine.InputSystem;

public enum WeaponCycleDirection
{
    Next,
    Previous,
}

[RequireComponent(typeof(CharacterController), typeof(WeaponManager), typeof(Health))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    PlayerCharacterRuntimeSet _playerCharacterRuntimeSet = null;

    [Header("Child Entity References")]
    [SerializeField]
    Transform _cameraFollowTarget = null;
    [SerializeField]
    Camera _playerCamera = null;

    [Header("Movement Settings")]
    [SerializeField]
    float _moveSpeed = 1.0F;
    [SerializeField]
    float _sprintMultiplier = 1.5F;
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

    const float GRAVITY_ACCELERATION = 9.81F;
    const float GROUND_CHECK_EPSILON = 0.1F;

    CharacterController _characterController = null;
    WeaponManager _weaponManager = null;
    Health _characterHealth = null;
    Vector2 _moveInput = Vector2.zero;
    Vector2 _lookInput = Vector2.zero;

    bool _isGrounded = false;
    Vector3 _gravityVector = Vector3.zero;
    bool _justJumped = false;
    bool _isSprinting = false;
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

    void Awake()
    {
        _playerCharacterRuntimeSet.Add(this);
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _weaponManager = GetComponent<WeaponManager>();
        _characterHealth = GetComponent<Health>();
        _characterHealth.OnHealthDepleted += OnHealthDepleted;
        GroundCheck();
    }

    void OnDestroy()
    {
        _playerCharacterRuntimeSet.Remove(this);
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
        _cameraFollowTarget.Rotate(Vector3.right, aimSpeedMultiplier * _lookInput.y);
        transform.Rotate(Vector3.up, aimSpeedMultiplier * _lookInput.x);

        Vector3 euler = _cameraFollowTarget.localEulerAngles;
        euler.z = 0;
        euler.y = 0;
        euler.x = euler.x < 180.0F ? Mathf.Clamp(euler.x, 0, 80) : Mathf.Clamp(euler.x, 280, 360);
        _cameraFollowTarget.localEulerAngles = euler;

        // Move character
        float actualMoveSpeed = _moveSpeed;
        if (_isSprinting && _isGrounded && !_isAiming)
        {   // Only apply sprint speed when grounded and not aiming
            actualMoveSpeed *= _sprintMultiplier;
        }

        Vector3 moveDirection = transform.forward * _moveInput.y + transform.right * _moveInput.x;
        Vector3 moveVector = moveDirection * actualMoveSpeed * Time.deltaTime;
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

        // Lastly manage player state
        if (_isAiming || _isShooting)
        {   // Cannot sprint while shooting or aiming
            _isSprinting = false;
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
        Vector3 origin = transform.position + _characterController.center;
        if (Physics.SphereCast(origin, checkRadius, Vector3.down, out RaycastHit _, checkDistance, mask))
        {
            _isGrounded = true;
            _gravityVector = Vector3.zero;
        }
    }

    private void OnHealthDepleted()
    {
        Debug.Log("Owno, the player died :(");
        Destroy(gameObject);
    }

    public void SetMoveDirection(Vector2 direction)
    {
        _moveInput = direction;
    }

    public void SetLookRotation(Vector2 rotation)
    {
        _lookInput = rotation;
    }

    public void ToggleSprint()
    {
        _isSprinting = !_isSprinting;
    }

    public void SetSprintState(bool isSprinting)
    {
        _isSprinting = isSprinting;
    }

    public void Jump()
    {
        _justJumped = true;
    }

    public void Reload()
    {
        _weaponManager.Reload();
    }

    public void CycleWeapon(WeaponCycleDirection direction)
    {
        if (direction == WeaponCycleDirection.Next)
        {
            _weaponManager.SelectNextWeapon();
        }
        
        if (direction == WeaponCycleDirection.Previous)
        {
            _weaponManager.SelectPreviousWeapon();
        }
    }

    public void SetAimState(bool isAiming)
    {
        _isAiming = isAiming;
    }

    public void Shoot()
    {
        _isShooting = true;
        _stoppedShooting = false;
    }

    public void ShootReset()
    {
        _isShooting = false;
        _stoppedShooting = true;
    }
}
