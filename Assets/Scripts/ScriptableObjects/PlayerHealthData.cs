using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthData", menuName = "Game/Player/Health Data")]
public class PlayerHealthData : ScriptableObject
{
    public int maxHealth = 3;
    public float invincibilityDuration = 2f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
}
