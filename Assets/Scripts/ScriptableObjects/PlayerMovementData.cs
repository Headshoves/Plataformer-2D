using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementData", menuName = "Game/Player/Movement Data")]
public class PlayerMovementData : ScriptableObject
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 10f;
    public float airControlFactor = 0.5f;
    public float groundCheckDistance = 0.2f; // Aumentado para melhor detecção
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float jumpForce = 10f;
    public int maxJumps = 2;
}
