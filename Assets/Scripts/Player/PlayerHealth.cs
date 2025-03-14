using UnityEngine;

/// <summary>
/// Gerencia o sistema de vida e dano do jogador
/// </summary>
public class PlayerHealth
{
    // Referências
    private PlayerController controller;    // Controlador principal
    private PlayerHealthData data;          // Dados de configuração de vida
    
    // Estados de vida
    private int currentHealth;              // Vida atual
    private bool isInvincible;             // Estado de invencibilidade
    private float invincibilityTimer;      // Tempo restante de invencibilidade
    private bool isStunned;                // Estado de atordoamento
    private float stunTimer;               // Tempo restante de atordoamento

    /// <summary>
    /// Propriedades públicas para acesso ao estado
    /// </summary>
    public bool IsStunned => isStunned;
    public int CurrentHealth => currentHealth;

    public PlayerHealth(PlayerController controller, PlayerHealthData data)
    {
        this.controller = controller;
        this.data = data;
        currentHealth = data.maxHealth;
    }

    public void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        GameEvents.Instance.TriggerEvent("OnHealthChanged", currentHealth);

        ApplyKnockback(knockbackDirection);
        StartInvincibility();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Desativa o movimento do jogador
        var rb = controller.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }

        // Dispara o evento de morte
        GameEvents.Instance.TriggerEvent("OnPlayerDeath");
    }

    public void TakeDamageWithKnockback(int damage, float invincibilityDuration, Vector2 knockbackDirection, float knockbackForce, float knockbackDuration)
    {
        if (isInvincible) return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
        GameEvents.Instance.TriggerEvent("OnHealthChanged", currentHealth);

        ApplyKnockback(knockbackDirection * knockbackForce);
        StartInvincibility(invincibilityDuration);

        if (currentHealth <= 0)
            GameEvents.Instance.TriggerEvent("OnPlayerDeath");
    }

    public void AddHealth(int amount)
    {
        currentHealth = Mathf.Min(data.maxHealth, currentHealth + amount);
        GameEvents.Instance.TriggerEvent("OnHealthChanged", currentHealth);
    }

    public void ActivateInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;
        GameEvents.Instance.TriggerEvent("OnInvincibilityStarted", duration);
    }

    private void ApplyKnockback(Vector2 direction)
    {
        isStunned = true;
        stunTimer = data.knockbackDuration;
        controller.GetComponent<Rigidbody2D>().linearVelocity = direction * data.knockbackForce;
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = data.invincibilityDuration;
        GameEvents.Instance.TriggerEvent("OnInvincibilityStarted", data.invincibilityDuration);
    }

    private void StartInvincibility(float duration)
    {
        isInvincible = true;
        invincibilityTimer = duration;
        GameEvents.Instance.TriggerEvent("OnInvincibilityStarted", duration);
    }

    public void Update()
    {
        UpdateInvincibility();
        UpdateStun();
    }

    private void UpdateInvincibility()
    {
        if (!isInvincible) return;
        
        invincibilityTimer -= Time.deltaTime;
        if (invincibilityTimer <= 0)
            isInvincible = false;
    }

    private void UpdateStun()
    {
        if (!isStunned) return;

        stunTimer -= Time.deltaTime;
        if (stunTimer <= 0)
            isStunned = false;
    }
}
