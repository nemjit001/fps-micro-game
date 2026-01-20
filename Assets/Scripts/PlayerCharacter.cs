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

    [Header("Input Settings")]
    [SerializeField]
    float _lookSensitivity = 0.5F;
    [SerializeField]
    bool _invertYLook = false;

    CharacterController _characterController = null;
    Vector2 _rawMoveInput = Vector2.zero;
    Vector2 _rawLookInput = Vector2.zero;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Apply look rotation to character
        float yLookMultiplier = _invertYLook ? -1.0F : 1.0F;
        _cameraTransform.Rotate(Vector3.right, yLookMultiplier * _lookSensitivity * _rawLookInput.y);
        transform.Rotate(Vector3.up, _lookSensitivity * _rawLookInput.x);

        Vector3 euler = _cameraTransform.eulerAngles;
        euler.x = euler.x < 180.0F ? Mathf.Clamp(euler.x, 0, 80) : Mathf.Clamp(euler.x, 280, 360);
        _cameraTransform.eulerAngles = euler;

        // Move character
        Vector3 moveDirection = transform.forward * _rawMoveInput.y + transform.right * _rawMoveInput.x;
        Vector3 moveVector = moveDirection * _moveSpeed * Time.deltaTime;
        _characterController.Move(moveVector);
    }

    public void OnMove(InputValue value)
    {
        _rawMoveInput = value.Get<Vector2>();
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
