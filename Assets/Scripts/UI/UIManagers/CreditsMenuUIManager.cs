using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsMenuUIManager : UIManager
{
    [SerializeField]
    GameCredits _gameCredits = null;

    public Action OnLeaveMenu = null;
    Button _backButton = null;
    ListView _creditsList = null;

    void OnEnable()
    {
        UIDocument document = GetComponent<UIDocument>();
        _backButton = document.rootVisualElement.Query<Button>("back-button");
        _backButton.clicked += OnPressBack;

        _creditsList = document.rootVisualElement.Query<ListView>("credits-list");
        _creditsList.dataSource = _gameCredits;
        PopulateCreditsList();
    }

    void OnDisable()
    {
        _backButton.clicked -= OnPressBack;
    }

    private void OnPressBack()
    {
        OnLeaveMenu();
    }

    private void PopulateCreditsList()
    {
        if (_gameCredits == null || _creditsList == null)
        {
            return;
        }
        
        // TODO(nemjit001): populate credits list w/ credits elements
    }
}
