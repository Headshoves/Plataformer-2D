using UnityEngine;

[CreateAssetMenu(fileName = "HealthItemData", menuName = "Game/Item Data/Health Item")]
public class HealthItemData : ItemData
{
    public int healthAmount = 1;

    public override void ApplyEffect(PlayerController player)
    {
        player.AddHealth(healthAmount);
    }
}
