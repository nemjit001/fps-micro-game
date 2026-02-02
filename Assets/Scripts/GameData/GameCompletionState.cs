using UnityEngine;

enum CompletionCondition
{
    Win,
    Lose,
}

[CreateAssetMenu(fileName = "GameCompletionState", menuName = "Scriptable Objects/Runtime/GameCompletionState")]
class GameCompletionState : ScriptableObject
{
    [SerializeField]
    public CompletionCondition completionCondition = CompletionCondition.Win;
}
