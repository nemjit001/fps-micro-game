using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField]
    PauseMenuSystem _pauseMenu = null;

    static PauseManager _instance = null;

    public static PauseManager Instance
    {
        get => _instance;
    }

    public PauseMenuSystem PauseMenu { get => _pauseMenu; }

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
        _pauseMenu.Show();
        PlayerManager.Instance.SetActiveControls(ActiveControls.Paused);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1.0F;
        _pauseMenu.Hide();
        PlayerManager.Instance.SetActiveControls(ActiveControls.Gameplay);
    }
}
