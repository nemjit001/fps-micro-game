using UnityEngine;

[CreateAssetMenu(fileName = "AIChaseBrain", menuName = "Scriptable Objects/AI/AIChaseBrain")]
public class AIChaseBrain : AIBrain
{
    [Header("AI Settings")]
    [SerializeField]
    PlayerCharacterRuntimeSet _playerCharacterRuntimeSet = null;
    [SerializeField, Tooltip("Distance for target positions to be considered 'reached'")]
    float _targetReachedRadius = 1.0F;

    PlayerCharacter _playerTarget = null;

    public override void Think(EnemyCharacter character)
    {
        if (_playerCharacterRuntimeSet.items.Count == 0)
        {
            return;
        }

        AcquireTarget();
        if (IsTargetReached(character))
        {
            character.StopMoving();
            character.Attack();
        }
        else
        {
            character.MoveToTarget(_playerTarget.transform.position);            
        }
    }

    private void AcquireTarget()
    {
        if (_playerTarget == null)
        {
            _playerTarget = _playerCharacterRuntimeSet.items[Random.Range(0, _playerCharacterRuntimeSet.items.Count)];
        }
    }

    private bool IsTargetReached(EnemyCharacter character)
    {
        Vector3 positionDelta = character.transform.position - _playerTarget.transform.position;
        return Mathf.Sqrt(Vector3.Dot(positionDelta, positionDelta)) <= _targetReachedRadius;
    }
}
