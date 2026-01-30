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
    [SerializeField, Tooltip("Distance for target positions to be considered 'reached'")]
    float _targetReachedRadius = 1.0F;
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
        _navMeshAgent.isStopped = true;
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
        Vector3 positionDelta = transform.position - position;
        if (Mathf.Sqrt(Vector3.Dot(positionDelta, positionDelta)) <= _targetReachedRadius)
        {
            _navMeshAgent.isStopped = true;
            return;
        }

        _navMeshAgent.isStopped = false;
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
