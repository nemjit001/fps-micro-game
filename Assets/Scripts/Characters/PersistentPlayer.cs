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

    public void OnMove(InputValue value)
    {
        //
    }

    public void OnLook(InputValue value)
    {
        //
    }

    public void OnSprint(InputValue value)
    {
        //
    }

    public void OnJump()
    {
        //
    }

    public void OnReload()
    {
        //
    }

    public void OnCycleWeapon(InputValue value)
    {
        //
    }

    public void OnAim(InputValue value)
    {
        //
    }

    public void OnShoot(InputValue value)
    {
        //
    }

    public void OnPause()
    {
        //
    }

    public void OnUnpause()
    {
        //
    }
}
