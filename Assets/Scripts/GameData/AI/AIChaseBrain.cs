using UnityEngine;

[CreateAssetMenu(fileName = "AIChaseBrain", menuName = "Scriptable Objects/AI/AIChaseBrain")]
public class AIChaseBrain : AIBrain
{
    [Header("AI Settings")]
    [SerializeField]
    PlayerRuntimeSet _playerRuntimeSet = null;

    public override void Think(EnemyCharacter character)
    {
        if (_playerRuntimeSet.items.Count == 0)
        {
            return;
        }

        PlayerCharacter player = _playerRuntimeSet.items[Random.Range(0, _playerRuntimeSet.items.Count)];
        character.MoveToTarget(player.transform.position);
    }
}
