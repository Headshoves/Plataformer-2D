using UnityEngine;

/// <summary>
/// Base class for all collectible items in the game
/// </summary>
public abstract class Item : MonoBehaviour, ICollectible
{
    [SerializeField] protected string itemName;
    [SerializeField] protected string description;
    [SerializeField] protected GameObject collectEffect;

    /// <summary>
    /// Called when the item is collected by the player
    /// </summary>
    public abstract void OnCollect(PlayerController collector);
    
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                if (collectEffect != null)
                {
                    ObjectPool.Instance.SpawnFromPool(collectEffect, transform.position, Quaternion.identity);
                }
                
                OnCollect(player);
                ObjectPool.Instance.ReturnToPool(gameObject);
            }
        }
    }

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
    }
}
