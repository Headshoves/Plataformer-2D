using UnityEngine;

[CreateAssetMenu(fileName = "InvincibilityItemData", menuName = "Game/Item Data/Invincibility Item")]
public class InvincibilityItemData : ItemData
{
    public float duration = 5f;

    public override void ApplyEffect(PlayerController player)
    {
        player.ActivateInvincibility(duration);
    }
}
