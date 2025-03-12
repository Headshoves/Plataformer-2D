using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour{
    [Header("Preferences")] 
    public Player_MovementStats MoveStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;
    
    private Rigidbody2D _rigidbody;
    
    //Movimento
    private Vector2 _moveVelocity;
    private bool _isFacingRight;
    
    //Colisao
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    private void Awake(){
        _isFacingRight = true;
        
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate(){
        CollisionChecks();

        if (_isGrounded){
            Move(MoveStats.GroundAcceleration, MoveStats.GroundDeceleration, Input_Manager.Movement);
        }
        else{
            Move(MoveStats.AirAcceleration, MoveStats.AirDeceleration, Input_Manager.Movement);
        }
    }

    #region MOVIMENTACAO

    private void Move(float acceleration, float deceleration, Vector2 moveInput){
        if (moveInput != Vector2.zero){
            
            TurnCheck(moveInput);
            
            Vector2 targetVelocity = Vector2.zero;
            if (Input_Manager.RunIsHeld){
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxRunSpeed;
            }
            else{
                targetVelocity = new Vector2(moveInput.x, 0f) * MoveStats.MaxWalkSpeed;
            }
            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rigidbody.velocity = new Vector2(_moveVelocity.x, _rigidbody.velocity.y);
        }
        else if (moveInput == Vector2.zero){
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rigidbody.velocity = new Vector2(_moveVelocity.x, _rigidbody.velocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput){
        if (_isFacingRight && moveInput.x < 0){
            Turn(false);
        }
        else if(!_isFacingRight && moveInput.x > 0){
            Turn(true);
        }
    }

    private void Turn(bool turnRight){
        if (turnRight){
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else{
            _isFacingRight = false;
            transform.Rotate(0f, -180f, 0f);
        }
    }

    #endregion

    #region COLISOES

    private void IsGrounded(){
        
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, MoveStats.GroundDetectionRayLenght);
        
        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down,MoveStats.GroundDetectionRayLenght, MoveStats.GroundLayer);

        if (_groundHit.collider != null){
            _isGrounded = true;
        }
        else{
            _isGrounded = false;
        }
    }

    private void CollisionChecks(){
        IsGrounded();
    }
    #endregion
}
