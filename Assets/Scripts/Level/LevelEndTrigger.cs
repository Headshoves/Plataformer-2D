using UnityEngine;

public class LevelEndTrigger : MonoBehaviour
{
    [Tooltip("Marque esta opção se esta for a última fase do jogo")]
    [SerializeField] private bool isFinalLevel = false;
    
    [Tooltip("Efeito visual para quando completar a fase")]
    [SerializeField] private ParticleSystem completionEffect;
    
    private bool triggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;
        
        if (other.CompareTag("Player"))
        {
            triggered = true;
            
            if (completionEffect != null)
            {
                completionEffect.Play();
            }
            
            GameManager.Instance.HandleLevelComplete(isFinalLevel);
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Adiciona aviso visual no editor
        if (isFinalLevel)
        {
            Debug.Log($"[{gameObject.name}] está marcado como fase final!");
        }
    }
#endif
}
