using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Health))]
public class EnemyCharacter : MonoBehaviour
{
    [Header("Runtime Data")]
    [SerializeField]
    EnemyRuntimeSet _enemyRuntimeSet = null;

    [Header("Child Entity References")]
    [SerializeField]
    Animator _animator = null;

    [Header("AI Settings")]
    [SerializeField]
    AIBrain _brain = null;

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
        // Update AI state
        if (_brain != null)
        {
            _brain.Think(this);
        }

        // Set animation state :)
        _animator.SetBool("IsMoving", !_navMeshAgent.isStopped);
    }

    /// <summary>
    /// Move the EnemyCharacter to a position.
    /// </summary>
    /// <param name="position"></param>
    public void MoveToTarget(Vector3 position)
    {
        _navMeshAgent.destination = position;
    }

    /// <summary>
    /// Kills off the enemy when its health is depleted.
    /// </summary>
    private void OnHealthDepleted()
    {
        Debug.Log("Owno I died :(");
        // TODO(nemjit001): Spawn death VFX :)
        Destroy(gameObject);
    }
}
