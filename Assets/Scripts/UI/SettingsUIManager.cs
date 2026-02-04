using UnityEngine;
using UnityEngine.UIElements;

public class SettingsUIManager : MonoBehaviour
{
    Button _controlsButton = null;
    Button _inputSettingsButton = null;
    Button _backButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _controlsButton = document.rootVisualElement.Query<Button>("controls-button");
        _inputSettingsButton = document.rootVisualElement.Query<Button>("input-settings-button");
        _backButton = document.rootVisualElement.Query<Button>("back-button");

        _controlsButton.clicked += OnPressControls;
        _inputSettingsButton.clicked += OnPressInputSettings;
        _backButton.clicked += OnPressBackButton;
    }

    void OnDisable()
    {
        _controlsButton.clicked -= OnPressControls;
        _inputSettingsButton.clicked -= OnPressInputSettings;
        _backButton.clicked -= OnPressBackButton;
    }

    private void OnPressControls()
    {
        //
    }

    private void OnPressInputSettings()
    {
        //
    }

    private void OnPressBackButton()
    {
        //
    }
}
