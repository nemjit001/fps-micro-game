using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameOverUIManager : MonoBehaviour
{
    Button _mainMenuButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _mainMenuButton = document.rootVisualElement.Query<Button>("main-menu-button");

        _mainMenuButton.clicked += OnPressMainMenu;
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
