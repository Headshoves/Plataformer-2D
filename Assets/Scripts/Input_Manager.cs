using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input_Manager : MonoBehaviour{
    public static Player_Input PlayerInput;

    public static Vector2 Movement;
    public static bool JumpWasPressed;
    public static bool JumpIsHeld;
    public static bool JumpWasReleased;
    public static bool RunIsHeld;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;

    private void Awake(){
        PlayerInput = GetComponent<Player_Input>();

        _moveAction = PlayerInput.FindAction("Move");
        _jumpAction = PlayerInput.FindAction("Jump");
        _runAction = PlayerInput.FindAction("Run");
    }

    private void Update(){
        Movement = _moveAction.ReadValue<Vector2>();

        JumpWasPressed = _jumpAction.WasCompletedThisFrame();
        JumpIsHeld = _runAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();
        
        RunIsHeld = _runAction.IsPressed();
    }
}
