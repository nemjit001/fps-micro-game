using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField]
    float _gameOverTimeout = 1.0F;
    [Header("Player Settings")]
    [SerializeField]
    PlayerCharacter _playerCharacter = null;
    [SerializeField]
    List<Transform> _playerSpawnPoints = new List<Transform>();

    bool _gameFinished = false;

    void Start()
    {
        SpawnPlayer();
    }

    void Update()
    {
        if (!_gameFinished && IsGameOver())
        {
            StartCoroutine(OnGameOverCoroutine());
        }
    }

    private void SpawnPlayer()
    {
        Transform spawnTransform = transform;
        if (_playerSpawnPoints.Count > 0)
        {
            spawnTransform = _playerSpawnPoints[Random.Range(0, _playerSpawnPoints.Count)];
        }

        PlayerCharacter newCharacter = Instantiate(_playerCharacter);
        newCharacter.transform.position = spawnTransform.position;
        newCharacter.transform.rotation = spawnTransform.rotation;
    }

    private bool IsGameOver()
    {
        return false;
    }

    private IEnumerator OnGameOverCoroutine()
    {
        _gameFinished = true;
        yield return new WaitForSeconds(_gameOverTimeout);
        SceneManager.LoadSceneAsync("GameOverScene", LoadSceneMode.Single);
    }
}
