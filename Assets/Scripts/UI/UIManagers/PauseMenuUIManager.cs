using System;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenuUIManager : UIManager
{
    public Action OnOpenSettings = null;
    public Action OnUnpause = null;

    Button _continueButton = null;
    Button _settingsButton = null;
    Button _quitButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _continueButton = document.rootVisualElement.Query<Button>("continue-button");
        _settingsButton = document.rootVisualElement.Query<Button>("settings-button");
        _quitButton = document.rootVisualElement.Query<Button>("quit-button");

        _continueButton.clicked += OnPressContinue;
        _settingsButton.clicked += OnPressSettings;
        _quitButton.clicked += OnPressQuit;
    }

    void OnDisable()
    {
        _continueButton.clicked -= OnPressContinue;
        _settingsButton.clicked -= OnPressSettings;
        _quitButton.clicked -= OnPressQuit;
    }

    private void OnPressContinue()
    {
        OnUnpause();
    }

    private void OnPressSettings()
    {
        OnOpenSettings();
    }

    private void OnPressQuit()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
