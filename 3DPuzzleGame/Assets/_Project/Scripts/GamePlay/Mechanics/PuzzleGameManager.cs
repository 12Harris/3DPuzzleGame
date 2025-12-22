using UnityEngine;

// ============================================================================
// GAME MANAGER - PUZZLE GAME SPECIFIC
// ============================================================================
public class PuzzleGameManager : Singleton<PuzzleGameManager>
{
    [Header("Game State")]
    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private float _gameTime = 0f;
    
    private bool _isPaused = false;

    protected override void Awake()
    {
        base.Awake();
        InputHandler.Instance.OnPausePressed += TogglePause;
    }

    void Update()
    {
        if (!_isPaused)
            _gameTime += Time.deltaTime;
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;
        Time.timeScale = _isPaused ? 0f : 1f;
        Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = _isPaused;
        
        UIManager.Instance?.TogglePanel("Pause");
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.Instance?.HidePanel("Pause");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance?.ReloadCurrentScene();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneLoader.Instance?.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        if (InputHandler.Instance)
            InputHandler.Instance.OnPausePressed -= TogglePause;
    }
}