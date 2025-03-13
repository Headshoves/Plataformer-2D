using UnityEngine;

public class CoinItem : Item
{
    [SerializeField] private int value = 1;
    
    private void Start()
    {
        if (value <= 0)
        {
            Debug.LogWarning($"Coin value should be positive on {gameObject.name}");
            value = 1;
        }
    }

    public override void OnCollect(PlayerController collector)
    {
        if (collector != null)
        {
            collector.AddPoints(value);
            GameEvents.Instance.TriggerEvent("OnCoinCollected", value);
        }
    }
}
