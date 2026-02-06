using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsMenuUIManager : UIManager
{
    [SerializeField]
    GameCredits _gameCredits;

    public Action OnLeaveMenu = null;
    Button _backButton = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _backButton = document.rootVisualElement.Query<Button>("back-button");
        _backButton.clicked += OnPressBack;

        ListView creditsList = document.rootVisualElement.Query<ListView>("credits-list");
        creditsList.itemsSource = _gameCredits.credits;
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
