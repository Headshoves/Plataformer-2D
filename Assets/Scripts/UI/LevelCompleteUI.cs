using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelCompleteUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject levelCompletePanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private TextMeshProUGUI levelCompleteText;
    
    private void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
            
        SetupButtons();
        RegisterEvents();
    }
    
    private void SetupButtons()
    {
        if (nextLevelButton != null)
            nextLevelButton.onClick.AddListener(OnNextLevelClicked);
            
        if (restartButton != null)
            restartButton.onClick.AddListener(OnRestartClicked);
            
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(OnMainMenuClicked);
    }
    
    private void RegisterEvents()
    {
        GameEvents.Instance.AddListener("OnLevelComplete", ShowLevelComplete);
        GameEvents.Instance.AddListener("OnLastLevelComplete", ShowGameComplete);
    }
    
    private void ShowLevelComplete()
    {
        levelCompletePanel.SetActive(true);
        levelCompleteText.text = "Level Complete!";
        nextLevelButton.gameObject.SetActive(true);
    }
    
    private void ShowGameComplete()
    {
        levelCompletePanel.SetActive(true);
        levelCompleteText.text = "Congratulations!\nYou completed the game!";
        nextLevelButton.gameObject.SetActive(false);
    }
    
    private void OnNextLevelClicked()
    {
        GameManager.Instance.LoadNextLevel();
    }
    
    private void OnRestartClicked()
    {
        GameManager.Instance.RestartLevel();
    }
    
    private void OnMainMenuClicked()
    {
        GameManager.Instance.LoadMainMenu();
    }
}
