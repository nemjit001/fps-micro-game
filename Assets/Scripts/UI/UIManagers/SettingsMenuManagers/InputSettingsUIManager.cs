using System;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class InputSettingsUIManager : UIManager
{
    [SerializeField]
    PlayerInputSettings _inputSettings = null;

    const string INPUT_SETTINGS_FILENAME = "input_settings.json";

    public Action OnLeaveMenu = null;
    Button _backButton = null;
    
    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _backButton = document.rootVisualElement.Query<Button>("back-button");
        _backButton.clicked += OnPressBackButton;

        VisualElement controlContainer = document.rootVisualElement.Query<VisualElement>("menu-control-container");
        controlContainer.dataSource = _inputSettings;

        loadInputSettings();
    }

    void OnDisable()
    {
        _backButton.clicked -= OnPressBackButton;
    }

    private void OnPressBackButton()
    {
        SaveInputSettings();
        OnLeaveMenu();
    }

    void loadInputSettings()
    {
        string filePath = $"{Application.persistentDataPath}/{INPUT_SETTINGS_FILENAME}";
        if (File.Exists(filePath))
        {
            string contents = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(contents, _inputSettings);
        }
    }

    private void SaveInputSettings()
    {
        string jsonData = JsonUtility.ToJson(_inputSettings);
        File.WriteAllText($"{Application.persistentDataPath}/{INPUT_SETTINGS_FILENAME}", jsonData);
    }
}
