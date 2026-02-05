using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsMenuUIManager : UIManager
{
    public Action OnLeaveMenu = null;
    Button _backButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _backButton = document.rootVisualElement.Query<Button>("back-button");
        _backButton.clicked += OnPressBack;
    }

    void OnDisable()
    {
        _backButton.clicked -= OnPressBack;
    }

    private void OnPressBack()
    {
        OnLeaveMenu();
    }
}
