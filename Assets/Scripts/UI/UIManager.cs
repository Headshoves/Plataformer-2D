using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gerencia a interface do usuário e suas atualizações
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Texto que mostra a pontuação")]
    public TextMeshProUGUI scoreText;
    
    [Tooltip("Texto que mostra a vida")]
    public TextMeshProUGUI healthText;
    
    /// <summary>
    /// Inicializa os listeners de eventos
    /// </summary>
    private void Start()
    {
        GameEvents.Instance.AddIntListener("OnScoreChanged", UpdateScore);
        GameEvents.Instance.AddIntListener("OnHealthChanged", UpdateHealth);
    }

    /// <summary>
    /// Atualiza o texto de pontuação
    /// </summary>
    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = $"Score: {score}";
    }

    /// <summary>
    /// Atualiza o texto de vida
    /// </summary>
    public void UpdateHealth(int health)
    {
        if (healthText != null)
            healthText.text = $"Health: {health}";
    }
}
