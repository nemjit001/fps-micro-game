using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    EnemyRuntimeSet _enemyRuntimeSet = null;

    [Header("Wave Settings")]
    [SerializeField]
    int _waveCount = 1;
    [SerializeField]
    int _enemiesPerWave = 1;
    [SerializeField]
    EnemyCharacter _enemyCharacter = null;
    [SerializeField]
    List<Transform> _enemySpawnPoints = new List<Transform>();

    int _waveCounter = 0;
    int _enemiesLeftToSpawn = 0;

    void Update()
    {
        // All enemies defeated, start next wave
        if (IsWaveOver())
        {
            _waveCounter += 1;
            _enemiesLeftToSpawn = _enemiesPerWave;
        }

        // Only spawn enemies if waves are not yet done
        if (!WavesFinished())
        {
            SpawnEnemy();
        }
    }

    public bool WavesFinished()
    {
        return _waveCounter > _waveCount;
    }

    private bool IsWaveOver()
    {
        return _enemiesLeftToSpawn == 0 && _enemyRuntimeSet.items.Count == 0;
    }

    private void SpawnEnemy()
    {
        // TODO(nemjit001): Limit number of enemies at the same time or add time between spawns
        if (_enemiesLeftToSpawn == 0)
        {
            return;
        }

        Transform spawnTransform = transform;
        if (_enemySpawnPoints.Count > 0)
        {
            spawnTransform = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
        }

        EnemyCharacter newEnemy = Instantiate(_enemyCharacter);
        newEnemy.transform.position = spawnTransform.position;
        newEnemy.transform.rotation = spawnTransform.rotation;
        _enemiesLeftToSpawn -= 1;
    }
}
