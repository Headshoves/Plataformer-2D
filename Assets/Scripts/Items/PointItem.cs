using UnityEngine;

public class PointItem : Item
{
    public int pointsValue = 100;

    public override void OnCollect(PlayerController player)
    {
        player.AddPoints(pointsValue);
    }
}
