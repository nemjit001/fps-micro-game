using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class EnemyCharacter : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    PlayerRuntimeSet _playerRuntimeSet = null;
    [SerializeField]
    EnemyRuntimeSet _enemyRuntimeSet = null;

    NavMeshAgent _navMeshAgent = null;
    Health _characterHealth = null;

    void Awake()
    {
        _enemyRuntimeSet.Add(this);        
    }

    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _characterHealth = GetComponent<Health>();
        _characterHealth.OnHealthDepleted += OnHealthDepleted;
    }

    void OnDestroy()
    {
        _enemyRuntimeSet.Remove(this);
    }

    void Update()
    {
        // TODO(nemjit001): Select single player character and chase
        // Might be fun if only on seeing player they get chased, otherwise patrol random positions in the navmesh
        // TODO(nemjit001): If within range of attack (check by collider) disable navigating and play attack anims
        PlayerCharacter target = _playerRuntimeSet.items[0];
        _navMeshAgent.destination = target.transform.position;
    }

    private void OnHealthDepleted()
    {
        Debug.Log("Owno I died :(");
        // TODO(nemjit001): Spawn death VFX :)

        gameObject.SetActive(false);
        Destroy(gameObject, 1.0F);
    }
}
