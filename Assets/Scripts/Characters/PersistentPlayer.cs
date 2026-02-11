using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    [SerializeField]
    PersistentPlayerRuntimeSet _persistentPlayerRuntimeSet = null;

    void Awake()
    {
        _persistentPlayerRuntimeSet.Add(this);
        DontDestroyOnLoad(this); // Ensure player stays alive for duration of game
    }

    void OnDestroy()
    {
        _persistentPlayerRuntimeSet.Remove(this);
    }
}
