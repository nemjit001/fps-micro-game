using UnityEngine;

public class MainMenuSystem : MenuSystem
{
    [SerializeField]
    MainMenuUIManager _mainMenuUI = null;
    [SerializeField]
    SettingsMenuSystem _settingsMenu = null;

    void OnEnable()
    {
        OpenMainMenu();
        _mainMenuUI.OnOpenSettings += OpenSettings;

        _settingsMenu.OnLeaveMenu += OpenMainMenu;
    }

    void OnDisable()
    {
        _mainMenuUI.OnOpenSettings -= OpenSettings;
        
        _settingsMenu.OnLeaveMenu -= OpenMainMenu;
    }

    private void OpenMainMenu()
    {
        _mainMenuUI.Show();
        _settingsMenu.Hide();
    }

    private void OpenSettings()
    {
        _mainMenuUI.Hide();
        _settingsMenu.Show();
    }
}
