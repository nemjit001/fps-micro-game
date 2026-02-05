using UnityEngine;

public class MainMenuSystem : MenuSystem
{
    [SerializeField]
    MainMenuUIManager _mainMenuUI = null;
    [SerializeField]
    CreditsMenuUIManager _creditsUI = null;
    [SerializeField]
    SettingsMenuSystem _settingsMenu = null;

    void OnEnable()
    {
        OpenMainMenu();
        _mainMenuUI.OnOpenSettings += OpenSettings;
        _mainMenuUI.OnOpenCredits += OpenCredits;
        _creditsUI.OnLeaveMenu += OpenMainMenu;
        _settingsMenu.OnLeaveMenu += OpenMainMenu;
    }

    void OnDisable()
    {
        _mainMenuUI.OnOpenSettings -= OpenSettings;
        _mainMenuUI.OnOpenCredits -= OpenCredits;
        _creditsUI.OnLeaveMenu -= OpenMainMenu;
        _settingsMenu.OnLeaveMenu -= OpenMainMenu;
    }

    private void OpenMainMenu()
    {
        _mainMenuUI.Show();
        _creditsUI.Hide();
        _settingsMenu.Hide();
    }

    private void OpenSettings()
    {
        _mainMenuUI.Hide();
        _creditsUI.Hide();
        _settingsMenu.Show();
    }

    private void OpenCredits()
    {
        _mainMenuUI.Hide();
        _creditsUI.Show();
        _settingsMenu.Hide();        
    }
}
