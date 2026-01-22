using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController), typeof(WeaponManager))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    Transform _cameraTransform = null;
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
    float _lookSensitivity = 0.5F;
    [SerializeField]
    bool _invertYLook = false;

    const float GRAVITY_ACCELERATION = 9.81F;
    const float GROUND_CHECK_EPSILON = 0.1F;

    CharacterController _characterController = null;
    WeaponManager _weaponManager = null;
    Vector2 _rawMoveInput = Vector2.zero;
    Vector2 _rawLookInput = Vector2.zero;

    bool _isGrounded = false;
    Vector3 _gravityVector = Vector3.zero;
    bool _justJumped = false;
    bool _isAiming = false;

    void OnValidate()
    {
        if (_playerCamera != null)
        {
            _playerCamera.fieldOfView = _defaultFOV;
        }
    }

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _weaponManager = GetComponent<WeaponManager>();
        GroundCheck();
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
        float yLookMultiplier = _invertYLook ? -1.0F : 1.0F;
        _cameraTransform.Rotate(Vector3.right, yLookMultiplier * _lookSensitivity * aimSpeedMultiplier * _rawLookInput.y);
        transform.Rotate(Vector3.up, _lookSensitivity * aimSpeedMultiplier * _rawLookInput.x);

        Vector3 euler = _cameraTransform.localEulerAngles;
        euler.z = 0;
        euler.y = 0;
        euler.x = euler.x < 180.0F ? Mathf.Clamp(euler.x, 0, 80) : Mathf.Clamp(euler.x, 280, 360);
        _cameraTransform.localEulerAngles = euler;

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
        float checkDistance = GROUND_CHECK_EPSILON + 0.5F * _characterController.height;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit _, checkDistance, ~0))
        {
            _isGrounded = true;
            _gravityVector = Vector3.zero;
        }
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
        //
    }

    public void OnAim(InputValue value)
    {
        _isAiming = value.Get<float>() > 0.5F;
    }

    public void OnShoot(InputValue value)
    {
        if (value.Get<float>() > 0.5F)
        {
            _weaponManager.Shoot(_playerCamera.transform);
        }
        else
        {
            //
        }
    }
}
