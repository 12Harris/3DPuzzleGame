// MainMenuManager.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    [Header("Main Menu Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    
    [Header("Options Panel Elements")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TMP_Dropdown graphicsDropdown;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Button backButton;
    
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI versionText;
    [SerializeField] private CanvasGroup mainMenuCanvasGroup;
    [SerializeField] private CanvasGroup optionsCanvasGroup;
    
    [Header("Scene Names")]
    [SerializeField] private string gameSceneName = "SampleScene";
    
    [Header("Animation Settings")]
    [SerializeField] private float fadeDuration = 0.3f;
    
    private void Start()
    {
        InitializeMenu();
        SetupButtonListeners();
        LoadPlayerPreferences();
        UpdateVersionText();
        CheckContinueButton();
    }
    
    private void InitializeMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        
        if (mainMenuCanvasGroup != null)
            mainMenuCanvasGroup.alpha = 1f;
        
        if (optionsCanvasGroup != null)
            optionsCanvasGroup.alpha = 0f;
    }
    
    private void SetupButtonListeners()
    {
        newGameButton.onClick.AddListener(OnNewGameClicked);
        continueButton.onClick.AddListener(OnContinueClicked);
        optionsButton.onClick.AddListener(OnOptionsClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        quitButton.onClick.AddListener(OnQuitClicked);
        backButton.onClick.AddListener(OnBackClicked);
        
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        graphicsDropdown.onValueChanged.AddListener(OnGraphicsChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
    }
    
    private void CheckContinueButton()
    {
        bool hasSaveData = PlayerPrefs.HasKey("SaveGame");
        continueButton.interactable = hasSaveData;
    }
    
    private void UpdateVersionText()
    {
        if (versionText != null)
            versionText.text = "v" + Application.version;
    }
    
    // Button Click Handlers
    private void OnNewGameClicked()
    {
        Debug.Log("Starting New Game");
        PlayerPrefs.DeleteKey("SaveGame");
        LoadGameScene();
    }
    
    private void OnContinueClicked()
    {
        Debug.Log("Continuing Game");
        LoadGameScene();
    }
    
    private void OnOptionsClicked()
    {
        Debug.Log("Opening Options");
        StartCoroutine(FadeToPanel(optionsPanel, mainMenuPanel));
    }
    
    private void OnCreditsClicked()
    {
        Debug.Log("Opening Credits");
        // Implement credits screen
    }
    
    private void OnQuitClicked()
    {
        Debug.Log("Quitting Game");
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    private void OnBackClicked()
    {
        Debug.Log("Returning to Main Menu");
        SavePlayerPreferences();
        StartCoroutine(FadeToPanel(mainMenuPanel, optionsPanel));
    }
    
    // Options Handlers
    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
    }
    
    private void OnGraphicsChanged(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }
    
    private void OnSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }
    
    // Save/Load Preferences
    private void LoadPlayerPreferences()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        graphicsDropdown.value = PlayerPrefs.GetInt("GraphicsQuality", 2);
        sensitivitySlider.value = PlayerPrefs.GetFloat("MouseSensitivity", 2f);
        
        AudioListener.volume = volumeSlider.value;
        QualitySettings.SetQualityLevel(graphicsDropdown.value);
    }
    
    private void SavePlayerPreferences()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("GraphicsQuality", graphicsDropdown.value);
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivitySlider.value);
        PlayerPrefs.Save();
    }
    
    // Scene Loading
    private void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    
    // Panel Transition
    private System.Collections.IEnumerator FadeToPanel(GameObject showPanel, GameObject hidePanel)
    {
        float elapsed = 0f;
        CanvasGroup showGroup = showPanel.GetComponent<CanvasGroup>();
        CanvasGroup hideGroup = hidePanel.GetComponent<CanvasGroup>();
        
        showPanel.SetActive(true);
        
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            
            if (hideGroup != null)
                hideGroup.alpha = 1f - t;
            
            if (showGroup != null)
                showGroup.alpha = t;
            
            yield return null;
        }
        
        if (hideGroup != null)
            hideGroup.alpha = 0f;
        
        if (showGroup != null)
            showGroup.alpha = 1f;
        
        hidePanel.SetActive(false);
    }
}