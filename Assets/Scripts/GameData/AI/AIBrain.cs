using UnityEngine;

/// <summary>
/// Base AI brain class, does not think. Specialize this to implement complex AI behaviours.
/// </summary>
[CreateAssetMenu(fileName = "AIBrain", menuName = "Scriptable Objects/AI/AIBrain")]
public class AIBrain : ScriptableObject
{
    public virtual void Think(EnemyCharacter character)
    {
        //
    }
}
