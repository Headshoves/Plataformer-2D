using UnityEngine;

/// <summary>
/// Principal controlador do jogador que gerencia todos os sistemas
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Configurações de movimento do jogador")]
    [SerializeField] private PlayerMovementData movementData;
    public PlayerMovementData MovementData => movementData;
    
    [Tooltip("Configurações de vida e dano do jogador")]
    [SerializeField] private PlayerHealthData healthData;
    
    [Tooltip("Layer que representa o chão para detecção")]
    [SerializeField] private LayerMask groundLayer;
    
    [Tooltip("Referência ao gerenciador de UI")]
    [SerializeField] private UIManager uiManager;
    
    // Pontuação atual do jogador
    private int score;
    
    // Sistemas do jogador
    private PlayerMovement movement;  // Sistema de movimento
    private PlayerHealth health;      // Sistema de vida
    private PlayerCombat combat;      // Sistema de combate
    private PlayerInput input;        // Sistema de input
    
    public int Score => score;
    public int Health => health.CurrentHealth;
    public PlayerMovement Movement => movement;
    
    private void Awake()
    {
        movement = new PlayerMovement(this, GetComponent<Rigidbody2D>(), movementData, groundLayer);  // Modificado
        health = new PlayerHealth(this, healthData);
        combat = new PlayerCombat(this);
        input = new PlayerInput();
        if (uiManager == null)
            uiManager = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        score = 0;
        
        // Dispara os eventos iniciais para atualizar a UI
        GameEvents.Instance.TriggerEvent("OnScoreChanged", score);
        GameEvents.Instance.TriggerEvent("OnHealthChanged", health.CurrentHealth);
    }

    private void Update()
    {
        health.Update();
        if (health.IsStunned) return;
        
        input.ProcessInput();
        movement.HandleMovement(input.MoveInput, input.IsRunning);
        movement.HandleJump(input.JumpPressed, input.JumpReleased);
    }

    private void FixedUpdate()
    {
        if (health.IsStunned) return;
        movement.FixedUpdate();
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        health.TakeDamage(damage, knockbackDirection);
    }

    public void TakeDamageWithKnockback(int damage, float invincibilityDuration, Vector2 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        health.TakeDamageWithKnockback(damage, invincibilityDuration, knockbackDirection, knockbackForce, knockbackDuration);
    }

    public void AddPoints(int points)
    {
        score += points;
        GameEvents.Instance.TriggerEvent("OnScoreChanged", score);
        if (uiManager != null)
            uiManager.UpdateScore(score);
    }

    public void AddHealth(int amount)
    {
        health.AddHealth(amount);
        if (uiManager != null)
            uiManager.UpdateHealth(health.CurrentHealth);
    }

    public void ActivateInvincibility(float duration)
    {
        health.ActivateInvincibility(duration);
    }

    public void Bounce()
    {
        combat.Bounce();
    }
}
