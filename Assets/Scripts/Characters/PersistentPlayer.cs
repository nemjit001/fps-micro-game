using UnityEngine;
using UnityEngine.InputSystem;

public class PersistentPlayer : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField]
    PlayerInputSettings _inputSettings = null;

    [Header("Runtime Data")]
    [SerializeField]
    PersistentPlayerRuntimeSet _persistentPlayerRuntimeSet = null;

    PlayerInput _playerInput = null;
    PlayerCharacter _character = null;

    void Awake()
    {
        _persistentPlayerRuntimeSet.Add(this);
        DontDestroyOnLoad(this); // Ensure player stays alive for duration of the game
    }

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
    }

    void OnDestroy()
    {
        _persistentPlayerRuntimeSet.Remove(this);
    }

    public void Possess(PlayerCharacter character)
    {
        _character = character;
    }

    public void OnMove(InputValue value)
    {
        _character.SetMoveDirection(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
        Vector2 rotation = value.Get<Vector2>();
        rotation.y *= _inputSettings.invertYLook ? 1.0F : -1.0F;
        rotation *= _inputSettings.lookSensitivity;

        _character.SetLookRotation(rotation);
    }

    public void OnSprint(InputValue value)
    {
        if (_inputSettings.toggleSprint)
        {
            _character.ToggleSprint();
        }
        else
        {
            _character.SetSprintState(value.Get<float>() > 0.5F);
        }
    }

    public void OnJump()
    {
        _character.Jump();
    }

    public void OnReload()
    {
        _character.Reload();
    }

    public void OnCycleWeapon(InputValue value)
    {
        if (value.Get<float>() > 0.0F)
        {
            _character.CycleWeapon(WeaponCycleDirection.Next);
        }
        else if (value.Get<float>() < 0.0F)
        {
            _character.CycleWeapon(WeaponCycleDirection.Previous);
        }
    }

    public void OnAim(InputValue value)
    {
        _character.SetAimState(value.Get<float>() > 0.5F);
    }

    public void OnShoot(InputValue value)
    {
        _character.SetShootState(value.Get<float>() > 0.5F);
    }

    public void OnPause()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.PauseGame();
            PauseManager.Instance.PauseMenu.OnUnpause += OnUnpause;
            _playerInput.SwitchCurrentActionMap("Paused");
        }
    }

    public void OnUnpause()
    {
        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.UnpauseGame();
            PauseManager.Instance.PauseMenu.OnUnpause -= OnUnpause;
            _playerInput.SwitchCurrentActionMap("Gameplay");
        }
    }
}
