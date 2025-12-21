// ============================================================================
// UI MANAGER
// ============================================================================
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _gameplayPanel;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _loadingPanel;

    [Header("HUD Elements")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _healthBar;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private Slider _loadingBar;

    private Dictionary<string, GameObject> _panels = new Dictionary<string, GameObject>();
    private string _currentPanel;

    protected override void Awake()
    {
        base.Awake();
        InitializePanels();
        SubscribeToEvents();
    }

    private void InitializePanels()
    {
        if (_mainMenuPanel) _panels["MainMenu"] = _mainMenuPanel;
        if (_gameplayPanel) _panels["Gameplay"] = _gameplayPanel;
        if (_pausePanel) _panels["Pause"] = _pausePanel;
        if (_settingsPanel) _panels["Settings"] = _settingsPanel;
        if (_loadingPanel) _panels["Loading"] = _loadingPanel;

        HideAllPanels();
    }

    private void SubscribeToEvents()
    {
        GameEvents.OnScoreChanged.Subscribe(UpdateScore);
        GameEvents.OnHealthChanged.Subscribe(UpdateHealth);
        GameEvents.OnLoadingProgress?.Subscribe(UpdateLoadingBar);
    }

    // Panel Management
    public void ShowPanel(string panelName)
    {
        if (_panels.TryGetValue(panelName, out GameObject panel))
        {
            HideAllPanels();
            panel.SetActive(true);
            _currentPanel = panelName;
        }
    }

    public void HidePanel(string panelName)
    {
        if (_panels.TryGetValue(panelName, out GameObject panel))
        {
            panel.SetActive(false);
            if (_currentPanel == panelName)
                _currentPanel = null;
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in _panels.Values)
            panel.SetActive(false);
        _currentPanel = null;
    }

    public void TogglePanel(string panelName)
    {
        if (_panels.TryGetValue(panelName, out GameObject panel))
        {
            if (panel.activeSelf)
                HidePanel(panelName);
            else
                ShowPanel(panelName);
        }
    }

    // HUD Updates
    public void UpdateScore(int score)
    {
        if (_scoreText)
            _scoreText.text = $"Score: {score}";
    }

    public void UpdateHealth(float health)
    {
        if (_healthBar)
            _healthBar.value = health;
    }

    public void ShowMessage(string message, float duration = 3f)
    {
        if (_messageText)
        {
            _messageText.text = message;
            _messageText.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(duration));
        }
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (_messageText)
            _messageText.gameObject.SetActive(false);
    }

    public void UpdateLoadingBar(float progress)
    {
        if (_loadingBar)
            _loadingBar.value = progress;
    }

    // Utility Methods
    public void SetButtonInteractable(Button button, bool interactable)
    {
        if (button)
            button.interactable = interactable;
    }

    public void FadePanel(GameObject panel, bool fadeIn, float duration = 0.5f)
    {
        StartCoroutine(FadePanelCoroutine(panel, fadeIn, duration));
    }

    private IEnumerator FadePanelCoroutine(GameObject panel, bool fadeIn, float duration)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (!canvasGroup)
            canvasGroup = panel.AddComponent<CanvasGroup>();

        float elapsed = 0f;
        float startAlpha = fadeIn ? 0f : 1f;
        float endAlpha = fadeIn ? 1f : 0f;

        panel.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
        if (!fadeIn)
            panel.SetActive(false);
    }
}

// ============================================================================
// EXAMPLE USAGE
// ============================================================================
/*
// Example: Scene Manager
SceneLoader.Instance.LoadScene("Level1", () => Debug.Log("Level loaded!"));
SceneLoader.Instance.ReloadCurrentScene();

// Example: Input Handler
void Start()
{
    InputHandler.Instance.OnJumpPressed += HandleJump;
    InputHandler.Instance.OnAttackPressed += HandleAttack;
}

void Update()
{
    Vector2 moveInput = InputHandler.Instance.MoveInput;
    // Use moveInput for character movement
}

void HandleJump() => Debug.Log("Jump!");
void HandleAttack() => Debug.Log("Attack!");

// Example: UI Manager
UIManager.Instance.ShowPanel("MainMenu");
UIManager.Instance.UpdateScore(100);
UIManager.Instance.UpdateHealth(0.75f);
UIManager.Instance.ShowMessage("Level Complete!", 3f);
UIManager.Instance.FadePanel(somePanel, true, 0.5f);

// Example: Using Singleton
public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        Debug.Log("GameManager initialized!");
    }
}

// Example: Using Object Pool
ObjectPool<Bullet> bulletPool = new ObjectPool<Bullet>(bulletPrefab, 20);
Bullet bullet = bulletPool.Get();
// ... use bullet
bulletPool.Return(bullet);

// Example: Using Events
GameEvents.OnScoreChanged.Subscribe(OnScoreUpdated);
GameEvents.OnScoreChanged.Invoke(100);

void OnScoreUpdated(int newScore)
{
    Debug.Log($"Score: {newScore}");
}

// Example: Using Timer
Timer cooldownTimer = new Timer(5f);
cooldownTimer.Start(() => Debug.Log("Ready!"));

void Update()
{
    cooldownTimer.Tick(Time.deltaTime);
}

// Example: Using Save System
[System.Serializable]
public class GameData
{
    public int score;
    public float health;
}

GameData data = new GameData { score = 100, health = 50f };
SaveSystem.Save(data);
GameData loaded = SaveSystem.Load<GameData>();
*/
/*
// Example: Using Singleton
public class GameManager : Singleton<GameManager>
{
    void Start()
    {
        Debug.Log("GameManager initialized!");
    }
}

// Example: Using Object Pool
ObjectPool<Bullet> bulletPool = new ObjectPool<Bullet>(bulletPrefab, 20);
Bullet bullet = bulletPool.Get();
// ... use bullet
bulletPool.Return(bullet);

// Example: Using Events
GameEvents.OnScoreChanged.Subscribe(OnScoreUpdated);
GameEvents.OnScoreChanged.Invoke(100);

void OnScoreUpdated(int newScore)
{
    Debug.Log($"Score: {newScore}");
}

// Example: Using Timer
Timer cooldownTimer = new Timer(5f);
cooldownTimer.Start(() => Debug.Log("Ready!"));

void Update()
{
    cooldownTimer.Tick(Time.deltaTime);
}

// Example: Using Save System
[System.Serializable]
public class GameData
{
    public int score;
    public float health;
}

GameData data = new GameData { score = 100, health = 50f };
SaveSystem.Save(data);
GameData loaded = SaveSystem.Load<GameData>();
*/