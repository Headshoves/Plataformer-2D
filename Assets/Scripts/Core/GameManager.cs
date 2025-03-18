using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private GameState currentState;
    public GameState CurrentState => currentState;

    [Tooltip("Número total de fases no jogo (excluindo menu)")]
    [SerializeField] private int totalLevels = 1;

    [Serializable]
    private class GameData
    {
        public int highScore;
        public int currentLevel;
    }

    private GameData gameData = new GameData();
    private string savePath;

    private GameState previousState;
    private bool isShowingInfoBoard = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Verifica se existe um GameEvents na cena
            if (FindObjectOfType<GameEvents>() == null)
            {
                // Se não existir, cria um novo GameObject com GameEvents
                GameObject eventSystem = new GameObject("GameEvents");
                eventSystem.AddComponent<GameEvents>();
                DontDestroyOnLoad(eventSystem);
            }

            savePath = Path.Combine(Application.persistentDataPath, "gamedata.json");
            LoadGame();
            
            // Inicializa o nível atual baseado na cena
            InitializeCurrentLevel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeCurrentLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName.StartsWith("Level_"))
        {
            string levelNumber = currentSceneName.Replace("Level_", "");
            if (int.TryParse(levelNumber, out int level))
            {
                gameData.currentLevel = level - 1; // Ajusta para 0-based
            }
        }
        else
        {
            gameData.currentLevel = 0;
        }
    }

    private void Start()
    {
        // Move o registro do evento para o Start para garantir que o GameEvents já está inicializado
        GameEvents.Instance.AddListener("OnPlayerDeath", HandlePlayerDeath);
        GameEvents.Instance.AddListener("OnPlayerFallDeath", HandlePlayerFallDeath);
        currentState = GameState.Playing;
    }

    private void OnEnable()
    {
        // Registra novamente o evento quando o objeto for reativado
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.AddListener("OnPlayerDeath", HandlePlayerDeath);
            GameEvents.Instance.AddListener("OnPlayerFallDeath", HandlePlayerFallDeath);
        }
    }

    private void OnDisable()
    {
        // Remove o registro do evento quando o objeto for desativado
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.RemoveListener("OnPlayerDeath", HandlePlayerDeath);
            GameEvents.Instance.RemoveListener("OnPlayerFallDeath", HandlePlayerFallDeath);
        }
    }

    private void HandlePlayerDeath()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        GameEvents.Instance.TriggerEvent("OnGameOver");
    }

    private void HandlePlayerFallDeath()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        GameEvents.Instance.TriggerEvent("OnGameOver");
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        Time.timeScale = 0f;
        GameEvents.Instance.TriggerEvent("OnGameOver");
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(gameData);
        File.WriteAllText(savePath, json);
    }

    private void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            gameData = JsonUtility.FromJson<GameData>(json);
        }
    }

    public void UpdateHighScore(int score)
    {
        if (score > gameData.highScore)
        {
            gameData.highScore = score;
            SaveGame();
        }
    }

    public void PauseGame()
    {
        if (currentState == GameState.Playing)
        {
            Time.timeScale = 0f;
            currentState = GameState.Paused;
            GameEvents.Instance.TriggerEvent("OnGamePaused");
        }
    }

    public void ResumeGame()
    {
        if (currentState == GameState.Paused)
        {
            Time.timeScale = 1f;
            currentState = GameState.Playing;
            GameEvents.Instance.TriggerEvent("OnGameResumed");
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        currentState = GameState.MainMenu;
        SceneManager.LoadScene("MainMenu"); // Certifique-se de que esta cena existe
    }

    public void HandleLevelComplete(bool isFinalLevel)
    {
        currentState = GameState.LevelComplete;
        Time.timeScale = 0f;
        
        if (isFinalLevel || gameData.currentLevel >= totalLevels - 1)  // -1 porque currentLevel é 0-based
        {
            GameEvents.Instance.TriggerEvent("OnLastLevelComplete");
        }
        else
        {
            // Incrementa após verificar se é o último nível
            gameData.currentLevel++;
            SaveGame();
            GameEvents.Instance.TriggerEvent("OnLevelComplete");
        }

        Debug.Log($"Level Complete. Current Level: {gameData.currentLevel}");
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        currentState = GameState.Playing;
        
        // Calcula próximo nível (1-based para nome da cena)
        int nextLevelNumber = gameData.currentLevel + 1;
        
        if (nextLevelNumber <= totalLevels)
        {
            string nextSceneName = $"Level_{nextLevelNumber}";
            SceneManager.LoadScene(nextSceneName);
            GameEvents.Instance.TriggerEvent("OnLevelLoaded", nextLevelNumber);
            Debug.Log($"Loading level: {nextSceneName}");
        }
        else
        {
            Debug.LogWarning($"Tentando carregar fase inexistente (Level_{nextLevelNumber}). Voltando ao menu.");
            LoadMainMenu();
        }
    }

    public void ShowInfoBoard(InfoBoardData data)
    {
        if (currentState != GameState.Playing) return;
        
        previousState = currentState;
        currentState = GameState.Paused;
        isShowingInfoBoard = true;
        Time.timeScale = 0f;
        GameEvents.Instance.TriggerEvent("OnInfoBoardShow", data);
    }

    public void CloseInfoBoard()
    {
        if (!isShowingInfoBoard) return;
        
        isShowingInfoBoard = false;
        StartCoroutine(ResumeAfterDelay());
        GameEvents.Instance.TriggerEvent("OnInfoBoardClose");
    }

    private IEnumerator ResumeAfterDelay()
    {
        yield return new WaitForSecondsRealtime(0.1f); // Pequeno delay
        Time.timeScale = 1f;
        currentState = previousState;
    }

    public void Update()
    {
        if (isShowingInfoBoard && 
            (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Submit")))
        {
            CloseInfoBoard();
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Garante que totalLevels seja sempre pelo menos 1
        if (totalLevels < 1)
        {
            totalLevels = 1;
            Debug.LogWarning("Total de fases não pode ser menor que 1. Valor ajustado automaticamente.");
        }
    }
#endif
}
