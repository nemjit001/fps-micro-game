using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    EnemyRuntimeSet _enemyRuntimeSet = null;

    bool _gameFinished = false;

    void Update()
    {
        if (!_gameFinished && IsGameOver())
        {
            StartCoroutine(OnGameOverCoroutine());
        }
    }

    private bool IsGameOver()
    {
        return _enemyRuntimeSet.items.Count == 0;
    }

    private IEnumerator OnGameOverCoroutine()
    {
        _gameFinished = true;
        yield return new WaitForSeconds(5.0F);
        SceneManager.LoadSceneAsync("GameOverScene", LoadSceneMode.Single);
    }
}
