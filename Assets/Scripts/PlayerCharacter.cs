using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacter : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    Transform _cameraTransform = null;

    [Header("Movement Settings")]
    [SerializeField]
    float _moveSpeed = 1.0F;
    [SerializeField]
    float _characterMass = 1.0F;

    [Header("Input Settings")]
    [SerializeField]
    float _lookSensitivity = 0.5F;
    [SerializeField]
    bool _invertYLook = false;

    const float GRAVITY_ACCELERATION = 9.81F;
    const float GROUND_CHECK_EPSILON = 0.1F;

    CharacterController _characterController = null;
    Vector2 _rawMoveInput = Vector2.zero;
    Vector2 _rawLookInput = Vector2.zero;

    bool _isGrounded = false;
    Vector3 _gravityVector = Vector3.zero;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        GroundCheck();
    }

    void Update()
    {
        // Check character groundedness
        GroundCheck();

        // Apply look rotation to character
        float yLookMultiplier = _invertYLook ? -1.0F : 1.0F;
        _cameraTransform.Rotate(Vector3.right, yLookMultiplier * _lookSensitivity * _rawLookInput.y);
        transform.Rotate(Vector3.up, _lookSensitivity * _rawLookInput.x);

        Vector3 euler = _cameraTransform.eulerAngles;
        euler.z = 0;
        euler.x = euler.x < 180.0F ? Mathf.Clamp(euler.x, 0, 80) : Mathf.Clamp(euler.x, 280, 360);
        _cameraTransform.eulerAngles = euler;

        // Move character
        Vector3 moveDirection = transform.forward * _rawMoveInput.y + transform.right * _rawMoveInput.x;
        Vector3 moveVector = moveDirection * _moveSpeed * Time.deltaTime;
        _characterController.Move(moveVector);

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
        Debug.Log(_rawMoveInput);
    }

    public void OnLook(InputValue value)
    {
        _rawLookInput = value.Get<Vector2>();
    }

    public void OnAim(InputValue value)
    {
        //
    }

    public void OnShoot(InputValue value)
    {
        //
    }
}
