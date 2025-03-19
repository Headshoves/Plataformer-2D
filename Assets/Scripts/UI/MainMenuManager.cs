using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject continuePanel;
    [SerializeField] private GameObject creditsPanel;
    
    [Header("Main Menu Buttons")]
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button quitButton;
    
    [Header("Continue Panel")]
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button backButton;
    [SerializeField] private TextMeshProUGUI continueText;
    
    private void Start()
    {
        SetupButtons();
        ShowMainMenu();
        
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
        if (continuePanel != null)
            continuePanel.SetActive(false);
    }
    
    private void SetupButtons()
    {
        if (playButton != null)
            playButton.onClick.AddListener(OnPlayButtonClicked);
        if (creditsButton != null)
            creditsButton.onClick.AddListener(OnCreditsButtonClicked);
        if (quitButton != null)
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueButtonClicked);
        if (newGameButton != null)
            newGameButton.onClick.AddListener(OnNewGameButtonClicked);
        if (backButton != null)
            backButton.onClick.AddListener(ShowMainMenu);
    }
    
    private void OnPlayButtonClicked()
    {
        if (GameManager.Instance.HasSavedProgress())
        {
            ShowContinuePanel();
        }
        else
        {
            GameManager.Instance.StartNewGame();
        }
    }
    
    private void ShowContinuePanel()
    {
        mainMenuPanel.SetActive(false);
        continuePanel.SetActive(true);
        
        if (continueText != null)
        {
            int level = GameManager.Instance.GetLastUnlockedLevel();
            continueText.text = $"Continuar do Nível {level}?";
        }
    }
    
    private void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        continuePanel.SetActive(false);
        if (creditsPanel != null)
            creditsPanel.SetActive(false);
    }
    
    private void OnContinueButtonClicked()
    {
        GameManager.Instance.ContinueGame();
    }
    
    private void OnNewGameButtonClicked()
    {
        GameManager.Instance.StartNewGame();
    }
    
    private void OnCreditsButtonClicked()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }
    
    private void OnQuitButtonClicked()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
