using System;
using UnityEngine;

public class SettingsMenuSystem : MenuSystem
{
    [SerializeField]
    SettingsUIManager _settingsUI = null;
    [SerializeField]
    InputSettingsUIManager _inputSettingsUI = null;

    public Action OnLeaveMenu = null;

    void OnEnable()
    {
        OpenSettings();
        _settingsUI.OnOpenInputSettings += OpenInputSettings;
        _settingsUI.OnLeaveMenu += LeaveMenu;

        _inputSettingsUI.OnLeaveMenu += OpenSettings;
    }

    void OnDisable()
    {
        _settingsUI.OnOpenInputSettings -= OpenInputSettings;
        _settingsUI.OnLeaveMenu -= LeaveMenu;

        _inputSettingsUI.OnLeaveMenu -= OpenSettings;
    }

    private void OpenSettings()
    {
        _settingsUI.Show();
        _inputSettingsUI.Hide();
    }

    private void OpenInputSettings()
    {
        _settingsUI.Hide();
        _inputSettingsUI.Show();
    }

    private void LeaveMenu()
    {
        OnLeaveMenu();
    }
}
