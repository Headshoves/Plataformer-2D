using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

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
    }

    private void OnEnable()
    {
        // Registra novamente o evento quando o objeto for reativado
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.AddListener("OnPlayerDeath", HandlePlayerDeath);
        }
    }

    private void OnDisable()
    {
        // Remove o registro do evento quando o objeto for desativado
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.RemoveListener("OnPlayerDeath", HandlePlayerDeath);
        }
    }

    private void HandlePlayerDeath()
    {
        // Pequeno delay antes de reiniciar
        Invoke("RestartGame", 1.5f);
    }

    private void RestartGame()
    {
        // Recarrega a cena atual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
