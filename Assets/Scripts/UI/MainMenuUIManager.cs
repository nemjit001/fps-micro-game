using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUIManager : MonoBehaviour
{
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
        _quitButton.clicked += OnPressQuit;
    }

    void OnDisable()
    {
        _playButton.clicked -= OnPressPlay;
        _quitButton.clicked -= OnPressQuit;
    }

    private void OnPressPlay()
    {
        SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
    }

    private void OnPressQuit()
    {
    #if UNITY_EDITOR
        EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
}
