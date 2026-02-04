using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUIManager : UIManager
{
    [SerializeField]
    GameCompletionState _gameCompletionState = null;

    Label _youWinLabel = null;
    Label _gameOverLabel = null;
    Button _mainMenuButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _youWinLabel = document.rootVisualElement.Query<Label>("you-win-label");
        _gameOverLabel = document.rootVisualElement.Query<Label>("game-over-label");
        _mainMenuButton = document.rootVisualElement.Query<Button>("main-menu-button");

        _mainMenuButton.clicked += OnPressMainMenu;

        // Show & hide correct menu text :)
        if (_gameCompletionState.completionCondition == CompletionCondition.Win)
        {
            _youWinLabel.RemoveFromClassList("hidden");
            _gameOverLabel.AddToClassList("hidden");
        }
        else
        {
            _youWinLabel.AddToClassList("hidden");
            _gameOverLabel.RemoveFromClassList("hidden");
        }
    }

    void OnDisable()
    {
        _mainMenuButton.clicked -= OnPressMainMenu;
    }

    private void OnPressMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
