using UnityEngine;

public class PatrolEnemy : Enemy
{
    [Header("Patrol Settings")]
    public float moveSpeed = 2f;
    public float patrolDistance = 3f;
    public LayerMask groundLayer;
    
    private Vector2 startPosition;
    private int direction = 1;
    private SpriteRenderer spriteRenderer;
    
    private void Start()
    {
        base.Start();  // Isso chamará ValidateColliders()
        startPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Update()
    {
        if (isDead) return;
        
        Vector2 movement = Vector2.right * direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
        
        if (Mathf.Abs(transform.position.x - startPosition.x) > patrolDistance)
        {
            direction *= -1;
            spriteRenderer.flipX = direction < 0;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);  // Usar a implementação da classe base
    }

    private void OnDrawGizmos()
    {
        // Desenha a área de patrulha
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Vector3 leftPoint = (Vector3)startPosition + Vector3.left * patrolDistance;
            Vector3 rightPoint = (Vector3)startPosition + Vector3.right * patrolDistance;
            
            Gizmos.DrawLine(leftPoint, rightPoint);
            Gizmos.DrawWireSphere(leftPoint, 0.2f);
            Gizmos.DrawWireSphere(rightPoint, 0.2f);
        }

        // Desenha as áreas de colisão
        base.OnDrawGizmos();
    }
}
