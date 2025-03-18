/// <summary>
/// Gerencia e processa os inputs do jogador
/// </summary>
public class PlayerInput
{
    // Propriedades de input
    public float MoveInput { get; private set; }     // Input de movimento horizontal
    public bool IsRunning { get; private set; }      // Estado de corrida
    public bool JumpPressed { get; private set; }    // Botão de pulo pressionado
    public bool JumpReleased { get; private set; }   // Botão de pulo solto
    public bool JumpHeld { get; private set; }       // Botão de pulo segurado

    public void ProcessInput()
    {
        // Não processa inputs se o jogo estiver pausado
        if (GameManager.Instance.CurrentState != GameState.Playing)
        {
            MoveInput = 0;
            IsRunning = false;
            JumpPressed = false;
            JumpReleased = false;
            JumpHeld = false;
            return;
        }

        MoveInput = UnityEngine.Input.GetAxisRaw("Horizontal");
        IsRunning = UnityEngine.Input.GetKey(UnityEngine.KeyCode.LeftShift);
        JumpPressed = UnityEngine.Input.GetButtonDown("Jump");
        JumpReleased = UnityEngine.Input.GetButtonUp("Jump");
        JumpHeld = UnityEngine.Input.GetButton("Jump");
    }
}
