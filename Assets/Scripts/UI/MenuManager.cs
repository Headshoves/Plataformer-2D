using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    
    [Header("Buttons")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    
    [Header("Game Over Buttons")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button gameOverMenuButton;

    private void Start()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        SetupButtonListeners();
        
        GameEvents.Instance.AddListener("OnGamePaused", ShowPauseMenu);
        GameEvents.Instance.AddListener("OnGameResumed", HidePauseMenu);
        GameEvents.Instance.AddListener("OnGameOver", ShowGameOverMenu);
    }

    private void SetupButtonListeners()
    {
        if (pauseButton != null)
            pauseButton.onClick.AddListener(OnPauseButtonClicked);
        
        if (resumeButton != null)
            resumeButton.onClick.AddListener(OnResumeButtonClicked);
        
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartButtonClicked);
        
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
        
        if (retryButton != null)
            retryButton.onClick.AddListener(OnRetryButtonClicked);
        
        if (gameOverMenuButton != null)
            gameOverMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
    }

    private void OnPauseButtonClicked()
    {
        GameManager.Instance.PauseGame();
    }

    private void OnResumeButtonClicked()
    {
        GameManager.Instance.ResumeGame();
    }

    private void OnRestartButtonClicked()
    {
        GameManager.Instance.RestartLevel();
    }

    private void OnMainMenuButtonClicked()
    {
        GameManager.Instance.LoadMainMenu();
    }

    private void OnRetryButtonClicked()
    {
        GameManager.Instance.RetryGame();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void ShowPauseMenu()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);
    }

    private void HidePauseMenu()
    {
        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);
    }

    private void ShowGameOverMenu()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            pauseMenuPanel?.SetActive(false);
        }
    }

    private void Update()
    {
        // Modifica o Update para não permitir pause durante game over
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.CurrentState != GameState.GameOver)
        {
            if (GameManager.Instance.CurrentState == GameState.Playing)
                GameManager.Instance.PauseGame();
            else if (GameManager.Instance.CurrentState == GameState.Paused)
                GameManager.Instance.ResumeGame();
        }
    }
}
