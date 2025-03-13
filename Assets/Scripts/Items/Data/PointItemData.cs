using UnityEngine;

[CreateAssetMenu(fileName = "PointItemData", menuName = "Game/Item Data/Point Item")]
public class PointItemData : ItemData
{
    public int pointsValue = 100;

    public override void ApplyEffect(PlayerController player)
    {
        player.AddPoints(pointsValue);
    }
}
