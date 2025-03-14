using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int damageAmount = 1;
    public float invincibilityDuration = 3f;
    public bool canBeStomped = true;
    public float stompThreshold = 0.5f;
    public float knockbackForce = 10f;
    public float knockbackDuration = 0.2f;
    public int scoreValue = 100; // Pontos que o jogador ganha ao derrotar o inimigo
}
