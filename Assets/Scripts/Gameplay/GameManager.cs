using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    float _gameOverTimeout = 1.0F;
    [SerializeField]
    GameCompletionState _gameCompletionState = null;

    [Header("Player Settings")]
    [SerializeField]
    PlayerCharacter _playerCharacter = null;
    [SerializeField]
    List<Transform> _playerSpawnPoints = new List<Transform>();

    [Header("Runtime Data")]
    [SerializeField]
    PersistentPlayerRuntimeSet _persistentPlayerRuntimeSet = null;
    [SerializeField]
    PlayerCharacterRuntimeSet _playerCharacterRuntimeSet = null;
    [SerializeField]
    WaveManager _waveManager = null;

    bool _gameFinished = false;

    void Start()
    {
        SpawnPlayers();
    }

    void Update()
    {
        if (!_gameFinished && IsGameOver())
        {
            SetCompletionState();
            StartCoroutine(OnGameOverCoroutine());
        }
    }

    private void SpawnPlayers()
    {
        foreach (PersistentPlayer player in _persistentPlayerRuntimeSet.items)
        {
            Transform spawnTransform = transform;
            if (_playerSpawnPoints.Count > 0)
            {
                int spawnIdx = Random.Range(0, _playerSpawnPoints.Count);
                Debug.Log($"Spawn Position {spawnIdx}");
                spawnTransform = _playerSpawnPoints[spawnIdx];
            }

            // Spawn character
            PlayerCharacter newCharacter = Instantiate(_playerCharacter);
            newCharacter.transform.position = spawnTransform.position;
            newCharacter.transform.rotation = spawnTransform.rotation;

            // Forward player controls
            player.Possess(newCharacter);
        }
    }

    private void SetCompletionState()
    {
        if (!_waveManager.WavesFinished())
        {
            return;
        }

        if (_playerCharacterRuntimeSet.items.Count != 0)
        {
            _gameCompletionState.completionCondition = CompletionCondition.Win;
        }
        else
        {
            _gameCompletionState.completionCondition = CompletionCondition.Lose;
        }
    }

    private bool IsGameOver()
    {
        return _waveManager.WavesFinished() || _playerCharacterRuntimeSet.items.Count == 0;
    }

    private IEnumerator OnGameOverCoroutine()
    {
        _gameFinished = true;
        yield return new WaitForSecondsRealtime(_gameOverTimeout);
        PlayerManager.Instance.SetActiveControls(ActiveControls.UI);
        SceneManager.LoadSceneAsync("GameOverScene", LoadSceneMode.Single);
    }
}
