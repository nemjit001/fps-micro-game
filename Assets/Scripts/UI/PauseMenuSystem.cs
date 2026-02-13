using System;
using UnityEngine;

public class PauseMenuSystem : MenuSystem
{
    [SerializeField]
    PauseMenuUIManager _pauseMenuUI = null;
    [SerializeField]
    SettingsMenuSystem _settingsMenu = null;

    public Action OnUnpause = null;

    void OnEnable()
    {
        OpenPauseMenu();
        _pauseMenuUI.OnUnpause += Unpause;
        _pauseMenuUI.OnOpenSettings += OpenSettings;
        _settingsMenu.OnLeaveMenu += OpenPauseMenu;
    }

    void OnDisable()
    {
        _pauseMenuUI.OnUnpause -= Unpause;
        _pauseMenuUI.OnOpenSettings -= OpenSettings;
        _settingsMenu.OnLeaveMenu -= OpenPauseMenu;
    }

    void OpenPauseMenu()
    {
        _pauseMenuUI.Show();
        _settingsMenu.Hide();
    }

    void OpenSettings()
    {
        _pauseMenuUI.Hide();
        _settingsMenu.Show();
    }

    private void Unpause()
    {
        OnUnpause();
    }
}
