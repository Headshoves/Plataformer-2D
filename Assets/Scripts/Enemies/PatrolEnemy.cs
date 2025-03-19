using UnityEngine;

public class PatrolEnemy : Enemy
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public LayerMask groundLayer;
    
    [Header("Ground Check")]
    public float groundCheckDistance = 1f;    // Aumentado para melhor detecção
    public float wallCheckDistance = 0.5f;    // Aumentado para detecção antecipada
    public float groundCheckOffset = 0.75f;   // Ajustado para verificar mais à frente
    public float heightCheckOffset = 0.1f;

    [Header("Debug")]
    public bool showDebugRays = true;
    public Color groundRayColor = Color.green;
    public Color wallRayColor = Color.red;
    public Color noGroundColor = Color.yellow;
    public Color hitColor = Color.white;
    
    private int direction = 1;
    private SpriteRenderer spriteRenderer;
    private bool isVisible = false;
    
    private void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    
    private void Update()
    {
        if (isDead) return;

        // Se não estiver visível, não faz nada
        if (!isVisible) return;

        Vector2 rayOrigin = (Vector2)transform.position + (Vector2.up * heightCheckOffset);
        Vector2 groundCheckStart = rayOrigin + (Vector2.right * direction * groundCheckOffset);
        
        RaycastHit2D groundCheck = Physics2D.Raycast(groundCheckStart, Vector2.down, groundCheckDistance, groundLayer);
        RaycastHit2D wallCheck = Physics2D.Raycast(rayOrigin, Vector2.right * direction, wallCheckDistance, groundLayer);

        if (showDebugRays)
        {
            Debug.DrawRay(groundCheckStart, Vector2.down * groundCheckDistance, 
                groundCheck.collider ? hitColor : noGroundColor);
            Debug.DrawRay(rayOrigin, Vector2.right * direction * wallCheckDistance,
                wallCheck.collider ? hitColor : wallRayColor);
        }

        if (!groundCheck.collider || wallCheck.collider)
        {
            direction *= -1;
            spriteRenderer.flipX = direction < 0;
        }

        Vector2 movement = Vector2.right * direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private void OnDrawGizmos()
    {
        if (!showDebugRays || !Application.isPlaying) return;

        Vector2 rayOrigin = (Vector2)transform.position + (Vector2.up * heightCheckOffset);
        Vector2 groundCheckStart = rayOrigin + (Vector2.right * direction * groundCheckOffset);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(rayOrigin, 0.1f);
        Gizmos.DrawWireSphere(groundCheckStart, 0.1f);

        base.OnDrawGizmos();
    }
}
