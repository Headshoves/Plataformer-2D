using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] private int healAmount = 20;

    private void Start()
    {
        if (healAmount <= 0)
        {
            Debug.LogWarning($"Heal amount should be positive on {gameObject.name}");
            healAmount = 1;
        }
    }

    public override void OnCollect(PlayerController collector)
    {
        if (collector != null)
        {
            collector.AddHealth(healAmount);
            GameEvents.Instance.TriggerEvent("OnHealthCollected", healAmount);
        }
    }
}
