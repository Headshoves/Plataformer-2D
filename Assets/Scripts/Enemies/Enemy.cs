using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public int damageAmount = 1;
    public float invincibilityDuration = 3f;
    public bool canBeStomped = true;
    public float stompThreshold = 0.5f; // Altura mínima para considerar um stomp

    [Header("Collision Debug")]
    public bool showCollisionDebug = true;
    public Color topCollisionColor = Color.green;
    public Color sideCollisionColor = Color.red;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;

    [Header("Collision References")]
    [SerializeField] protected BoxCollider2D mainCollider;  // Colisão lateral
    [SerializeField] protected BoxCollider2D topCollider;   // Colisão superior
    [SerializeField] protected bool debugColliders = true;  // Visualização dos colliders

    [SerializeField] protected EnemyData enemyData;

    protected bool isDead = false;
    protected enum CollisionDirection { None, Left, Right, Top }
    protected CollisionDirection lastCollisionDirection;

    protected virtual void Start()
    {
        ValidateColliders();
    }

    protected void ValidateColliders()
    {
        if (mainCollider == null)
        {
            mainCollider = GetComponents<BoxCollider2D>()[0];
            Debug.LogWarning($"Main Collider not set on {gameObject.name}, attempting to auto-assign.");
        }
        if (topCollider == null)
        {
            if (GetComponents<BoxCollider2D>().Length > 1)
                topCollider = GetComponents<BoxCollider2D>()[1];
            Debug.LogWarning($"Top Collider not set on {gameObject.name}, attempting to auto-assign.");
        }

        if (mainCollider == null || topCollider == null)
        {
            Debug.LogError($"Critical: Colliders not properly set on {gameObject.name}. Please assign both colliders in the inspector.");
        }
    }

    public virtual void TakeDamage()
    {
        // Usar object pooling ao invés de destruir
        isDead = true;
        ObjectPool.Instance.ReturnToPool(gameObject);
    }

    protected virtual void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmos()
    {
        if (!debugColliders) return;

        // Visualizar mainCollider
        if (mainCollider != null)
        {
            Gizmos.color = sideCollisionColor;
            DrawColliderGizmo(mainCollider);
        }

        // Visualizar topCollider
        if (topCollider != null)
        {
            Gizmos.color = topCollisionColor;
            DrawColliderGizmo(topCollider);
        }
    }

    private void DrawColliderGizmo(BoxCollider2D collider)
    {
        if (collider == null) return;
        Vector2 size = collider.size;
        Vector2 position = (Vector2)transform.position + collider.offset;
        Gizmos.DrawWireCube(position, size);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                HandlePlayerCollision(player, collision);
            }
        }
    }

    protected virtual void HandlePlayerCollision(PlayerController player, Collision2D collision)
    {
        if (isDead) return;

        ContactPoint2D contact = collision.GetContact(0);
        Collider2D hitCollider = contact.otherCollider;

        if (hitCollider == topCollider)
        {
            if (canBeStomped)
            {
                TakeDamage();
                player.Bounce();
                Debug.Log("Enemy stomped!");
            }
        }
        else if (hitCollider == mainCollider)
        {
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.TakeDamageWithKnockback(damageAmount, invincibilityDuration, knockbackDirection, knockbackForce, knockbackDuration);
            Debug.Log("Player hit from side with knockback");
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandlePlayerCollision(other.GetComponent<PlayerController>());
        }
    }

    protected virtual void HandlePlayerCollision(PlayerController player)
    {
        if (isDead || player == null) return;

        Vector2 hitPoint = player.transform.position;
        bool isTopHit = hitPoint.y > transform.position.y + enemyData.stompThreshold;

        if (isTopHit && enemyData.canBeStomped)
        {
            TakeDamage();
            player.Bounce();
        }
        else
        {
            Vector2 knockbackDirection = (player.transform.position - transform.position).normalized;
            player.TakeDamageWithKnockback(
                enemyData.damageAmount,
                enemyData.invincibilityDuration,
                knockbackDirection,
                enemyData.knockbackForce,
                enemyData.knockbackDuration
            );
        }
    }
}
