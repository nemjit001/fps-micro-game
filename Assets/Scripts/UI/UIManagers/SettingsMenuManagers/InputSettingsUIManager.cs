using System;
using UnityEngine.UIElements;

public class InputSettingsUIManager : UIManager
{
    public Action OnLeaveMenu = null;

    Button _backButton = null;
    
    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _backButton = document.rootVisualElement.Query<Button>("back-button");

        _backButton.clicked += OnPressBackButton;
    }

    void OnDisable()
    {
        _backButton.clicked -= OnPressBackButton;
    }

    private void OnPressBackButton()
    {
        OnLeaveMenu();
    }
}
