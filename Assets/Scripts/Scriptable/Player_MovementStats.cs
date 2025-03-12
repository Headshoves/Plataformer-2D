using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player Movement")]
public class Player_MovementStats : ScriptableObject{
    [Header("Walk")] 
    [Range(1f, 100f)] public float MaxWalkSpeed = 12.5f;
    [Range(0.25f, 50f)] public float GroundAcceleration = 5f;
    [Range(0.25f, 50f)] public float GroundDeceleration = 20f;
    [Range(0.25f, 50f)] public float AirAcceleration = 5f;
    [Range(0.25f, 50f)] public float AirDeceleration = 5f;
    
    [Header("Run")]
    [Range(1f, 100f)] public float MaxRunSpeed = 20f;
    
    [Header("Grounded/Collision Check")]
    public LayerMask GroundLayer;
    public float GroundDetectionRayLenght = 0.02f;
    public float HeadDetectionRayLenght = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = 0.75f;
}
