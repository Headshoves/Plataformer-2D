using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    private GameState currentState;
    public GameState CurrentState => currentState;

    [Serializable]
    private class GameData
    {
        public int highScore;
        public int currentLevel;
    }

    private GameData gameData = new GameData();
    private string savePath;

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
        }
        else
        {
            Destroy(gameObject);
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
}
