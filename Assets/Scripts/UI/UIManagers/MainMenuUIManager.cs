using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUIManager : UIManager
{
    public Action OnOpenSettings = null;
    public Action OnOpenCredits = null;

    Button _playButton = null;
    Button _settingsButton = null;
    Button _creditsButton = null;
    Button _quitButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _playButton = document.rootVisualElement.Query<Button>("play-button");
        _settingsButton = document.rootVisualElement.Query<Button>("settings-button");
        _creditsButton = document.rootVisualElement.Query<Button>("credits-button");
        _quitButton = document.rootVisualElement.Query<Button>("quit-button");

        _playButton.clicked += OnPressPlay;
        _settingsButton.clicked += OnPressSettings;
        _creditsButton.clicked += OnPressCredits;
        _quitButton.clicked += OnPressQuit;
    }

    void OnDisable()
    {
        _playButton.clicked -= OnPressPlay;
        _settingsButton.clicked -= OnPressSettings;
        _creditsButton.clicked -= OnPressCredits;
        _quitButton.clicked -= OnPressQuit;
    }

    private void OnPressPlay()
    {
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
    }

    private void OnPressSettings()
    {
        OnOpenSettings();
    }

    private void OnPressCredits()
    {
        OnOpenCredits();
    }

    private void OnPressQuit()
    {
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }
}
