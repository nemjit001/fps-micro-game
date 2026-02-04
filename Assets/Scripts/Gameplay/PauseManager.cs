using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    PauseMenuUIManager _pauseMenuUI = null;

    static PauseManager _instance = null;

    public static PauseManager Instance
    {
        get => _instance;
    }

    public PauseMenuUIManager UIManager { get => _pauseMenuUI; }

    public bool IsPaused { get => Time.timeScale == 0.0F; }

    void Awake()
    {
        if (_instance == null || _instance != this)
        {
            _instance = this;
        }
    }

    void Start()
    {
        UnpauseGame();
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0F;
        // _pauseMenuUI.Show();
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0F;
        // _pauseMenuUI.Hide();
    }
}
