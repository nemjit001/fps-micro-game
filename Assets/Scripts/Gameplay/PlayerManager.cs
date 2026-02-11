using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    PersistentPlayerRuntimeSet _persistentPlayerRuntimeSet = null;

    static PlayerManager _instance = null;
    PlayerInputManager _playerInputManager = null;

    public static PlayerManager Instance { get => _instance; }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();
        InitializePersistentPlayers();
    }

    private void InitializePersistentPlayers()
    {
        // Clean up previous player state
        foreach (PersistentPlayer player in _persistentPlayerRuntimeSet.items)
        {
            Destroy(player.gameObject);
        }

        // Spawn player
        _playerInputManager.JoinPlayer();
    }
}
