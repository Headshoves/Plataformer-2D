using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject collectEffect;
    public bool shouldRotate = true;
    public float rotationSpeed = 100f;

    public abstract void ApplyEffect(PlayerController player);
}
