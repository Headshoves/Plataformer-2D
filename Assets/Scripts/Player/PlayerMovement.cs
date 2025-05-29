using UnityEngine;

/// <summary>
/// Gerencia todo o sistema de movimento do jogador
/// </summary>
public class PlayerMovement
{
    public enum MovementState
    {
        Idle,
        Walking,
        Running,
        Jumping,
        Falling
    }

    private MovementState currentState = MovementState.Idle;
    public MovementState CurrentState => currentState;

    // Referências principais
    private PlayerController controller;    // Referência ao controlador principal
    private Rigidbody2D rb;                // Componente físico do jogador
    private PlayerMovementData data;        // Dados de configuração do movimento
    
    // Estados do movimento
    private bool isGrounded;               // Indica se está no chão
    private int jumpCount;                 // Contador de pulos realizados
    public int JumpCount => jumpCount;
    private float currentSpeed;            // Velocidade atual
    private float coyoteTimeCounter;       // Contador do coyote time
    private float jumpBufferCounter;       // Contador do buffer de pulo
    private bool facingRight = true;       // Direção que o jogador está olhando
    private LayerMask groundLayer;         // Layer do chão para detecção
    private bool wasGroundedLastFrame;     // Estado anterior de ground

    public PlayerMovement(PlayerController controller, Rigidbody2D rb, PlayerMovementData data, LayerMask groundLayer)
    {
        this.controller = controller;
        this.rb = rb;
        this.data = data;
        this.groundLayer = groundLayer;  // Adicionado
    }

    /// <summary>
    /// Processa o movimento horizontal do jogador
    /// </summary>
    public void HandleMovement(float moveInput, bool isRunning)
    {
        float targetSpeed = (isRunning ? data.runSpeed : data.walkSpeed) * moveInput;
        float controlFactor = isGrounded ? 1f : data.airControlFactor;

        if (moveInput != 0)
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, data.acceleration * controlFactor * Time.deltaTime);
        else
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, data.deceleration * controlFactor * Time.deltaTime);

        if ((currentSpeed > 0 && !facingRight) || (currentSpeed < 0 && facingRight))
            Flip();
    }

    /// <summary>
    /// Gerencia a lógica de pulo, incluindo coyote time e buffer
    /// </summary>
    public void HandleJump(bool jumpPressed, bool jumpReleased)
    {
        UpdateCoyoteTime();
        
        // Set jump buffer when jump is pressed
        if (jumpPressed)
        {
            jumpBufferCounter = data.jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (CanJump() && jumpBufferCounter > 0f)
        {
            PerformJump();
            jumpBufferCounter = 0f;
        }

        // Cut jump height if button is released early
        if (jumpReleased && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
        }
    }

    public void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
        CheckGround();
        UpdateMovementState();
    }

    private void Flip()
    {
        facingRight = !facingRight;
        controller.transform.localScale = new Vector3(
            controller.transform.localScale.x * -1,
            controller.transform.localScale.y,
            controller.transform.localScale.z
        );
    }

    private bool CanJump()
    {
        return (isGrounded || coyoteTimeCounter > 0f || jumpCount < data.maxJumps);
    }

    private void PerformJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, data.jumpForce);
        jumpCount++;
        coyoteTimeCounter = 0f;
        
        // Reset double jump trigger if we're doing a regular jump
        if (jumpCount == 1 && controller.GetComponent<Animator>() != null)
        {
            controller.GetComponent<Animator>().ResetTrigger("IsDoubleJumping");
        }
    }

    private void CheckGround()
    {
        wasGroundedLastFrame = isGrounded;
        
        // Cast rays from both feet
        Vector3 raycastOrigin = controller.transform.position;
        float raycastWidth = 0.25f; // Ajuste conforme o tamanho do seu personagem
        
        bool hitLeft = Physics2D.Raycast(
            raycastOrigin + Vector3.left * raycastWidth,
            Vector2.down,
            data.groundCheckDistance,
            groundLayer
        );
        
        bool hitRight = Physics2D.Raycast(
            raycastOrigin + Vector3.right * raycastWidth,
            Vector2.down,
            data.groundCheckDistance,
            groundLayer
        );
        
        bool hitCenter = Physics2D.Raycast(
            raycastOrigin,
            Vector2.down,
            data.groundCheckDistance,
            groundLayer
        );

        isGrounded = hitLeft || hitRight || hitCenter;

        // Debug rays
        Debug.DrawRay(raycastOrigin + Vector3.left * raycastWidth, Vector2.down * data.groundCheckDistance, Color.red);
        Debug.DrawRay(raycastOrigin + Vector3.right * raycastWidth, Vector2.down * data.groundCheckDistance, Color.red);
        Debug.DrawRay(raycastOrigin, Vector2.down * data.groundCheckDistance, Color.red);

        if (!wasGroundedLastFrame && isGrounded)
        {
            jumpCount = 0;
        }
    }

    private void UpdateCoyoteTime()
    {
        if (isGrounded)
        {
            coyoteTimeCounter = data.coyoteTime;
        }
        else if (coyoteTimeCounter > 0f)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void UpdateJumpBuffer()
    {
        if (jumpBufferCounter > 0f)
            jumpBufferCounter -= Time.deltaTime;
    }

    private void UpdateMovementState()
    {
        if (!isGrounded)
        {
            currentState = rb.linearVelocity.y > 0 ? MovementState.Jumping : MovementState.Falling;
        }
        else if (Mathf.Abs(currentSpeed) > 0.1f)
        {
            currentState = Mathf.Abs(currentSpeed) > data.walkSpeed ? 
                MovementState.Running : MovementState.Walking;
        }
        else
        {
            currentState = MovementState.Idle;
        }
    }
}
