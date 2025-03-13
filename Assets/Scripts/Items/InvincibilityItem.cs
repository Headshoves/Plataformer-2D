using UnityEngine;

public class InvincibilityItem : Item
{
    public float duration = 5f;

    public override void OnCollect(PlayerController player)
    {
        player.ActivateInvincibility(duration);
    }
}
