using UnityEngine;
using UnityEngine.UIElements;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField]
    Health _characterHealth = null;

    ProgressBar _playerHealthBar = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _playerHealthBar = document.rootVisualElement.Query<ProgressBar>("player-health-bar");
    }

    void OnDisable()
    {
        //
    }

    void Update()
    {
        // TODO(nemjit001): Show percentage based on current and max health
        _playerHealthBar.value = _characterHealth.CurrentHealth;
    }
}
