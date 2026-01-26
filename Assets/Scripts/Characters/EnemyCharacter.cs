using UnityEngine;

[RequireComponent(typeof(Health))]
public class EnemyCharacter : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    PlayerRuntimeSet _playerRuntimeSet = null;
    [SerializeField]
    EnemyRuntimeSet _enemyRuntimeSet = null;

    Health _characterHealth = null;

    void Start()
    {
        _characterHealth = GetComponent<Health>();
        _characterHealth.OnHealthDepleted += OnHealthDepleted;

        _enemyRuntimeSet.Add(this);
    }

    void OnDestroy()
    {
        _enemyRuntimeSet.Remove(this);
    }

    void Update()
    {
        // TODO(nemjit001): Set navmesh target to closest player position using player runtime set
    }

    private void OnHealthDepleted()
    {
        Debug.Log("Owno I died :(");
        // TODO(nemjit001): Spawn death VFX :)

        gameObject.SetActive(false);
        Destroy(gameObject, 1.0F);
    }
}
