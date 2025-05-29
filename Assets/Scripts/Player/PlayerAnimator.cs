using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerController))] // Adiciona requirement do PlayerController
public class PlayerAnimator : MonoBehaviour
{
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int IsRunning = Animator.StringToHash("IsRunning");
    private static readonly int IsJumping = Animator.StringToHash("IsJumping");
    private static readonly int IsDoubleJumping = Animator.StringToHash("IsDoubleJumping");
    private static readonly int IsFalling = Animator.StringToHash("IsFalling");

    private Animator animator;
    private PlayerMovement movement;
    private int currentJumpCount;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        var playerController = GetComponent<PlayerController>();
        
        if (animator == null)
        {
            Debug.LogError($"[PlayerAnimator] Animator não encontrado no GameObject {gameObject.name}!");
            enabled = false;
            return;
        }

        if (playerController == null)
        {
            Debug.LogError($"[PlayerAnimator] PlayerController não encontrado no GameObject {gameObject.name}!");
            enabled = false;
            return;
        }

        // Aguarda um frame para garantir que o PlayerController inicializou completamente
        StartCoroutine(InitializeAfterController(playerController));
    }

    private System.Collections.IEnumerator InitializeAfterController(PlayerController playerController)
    {
        yield return null; // Aguarda um frame

        movement = playerController.Movement;
        
        if (movement == null)
        {
            Debug.LogError($"[PlayerAnimator] PlayerMovement não encontrado no PlayerController!");
            enabled = false;
            yield break;
        }

        // Verificar se os parâmetros existem no Animator
        VerifyAnimatorParameters();
    }

    private void VerifyAnimatorParameters()
    {
        var requiredParameters = new[] { "IsWalking", "IsRunning", "IsJumping", "IsDoubleJumping", "IsFalling" };
        
        foreach (var paramName in requiredParameters)
        {
            bool found = false;
            foreach (var parameter in animator.parameters)
            {
                if (parameter.name == paramName)
                {
                    found = true;
                    Debug.Log($"[PlayerAnimator] Parâmetro encontrado: {paramName} ({parameter.type})");
                    break;
                }
            }
            
            if (!found)
            {
                Debug.LogError($"[PlayerAnimator] Parâmetro obrigatório não encontrado: {paramName}");
            }
        }
    }

    private void Update()
    {
        UpdateAnimationState();
    }

    private void UpdateAnimationState()
    {
        Debug.Log("Atualizando estado da animação...");
        if (movement == null || animator == null){
            Debug.LogWarning("PlayerMovement ou Animator não encontrados!");
            return;
        } 

        var state = movement.CurrentState;
        bool isWalking = state == PlayerMovement.MovementState.Walking;
        bool isRunning = state == PlayerMovement.MovementState.Running;
        bool isJumping = state == PlayerMovement.MovementState.Jumping;
        bool isFalling = state == PlayerMovement.MovementState.Falling;

        // Debug dos estados
        Debug.Log($"Estado Atual: {state} | Walking: {isWalking} | Running: {isRunning} | Jumping: {isJumping} | Falling: {isFalling}");

        animator.SetBool(IsWalking, isWalking);
        animator.SetBool(IsRunning, isRunning);
        animator.SetBool(IsJumping, isJumping);
        animator.SetBool(IsFalling, isFalling);
        
        // Detecta pulo duplo quando está no ar e faz um segundo pulo
        if (isJumping && movement.JumpCount > 1)
        {
            Debug.Log("Executando Double Jump!");
            animator.SetTrigger(IsDoubleJumping);
        }
    }
}
