using UnityEngine;
using System.Collections;

/// <summary>
/// Item que aumenta temporariamente a velocidade do jogador
/// </summary>
public class PowerUpItem : Item
{
    [Tooltip("Multiplicador de velocidade")]
    [SerializeField] private float speedBoostAmount = 1.5f;
    
    [Tooltip("Duração do power-up")]
    [SerializeField] private float duration = 5f;
    
    // Referência à coroutine ativa
    private Coroutine powerUpCoroutine;

    public override void OnCollect(PlayerController collector)
    {
        if (powerUpCoroutine != null)
        {
            StopCoroutine(powerUpCoroutine);
        }
        powerUpCoroutine = StartCoroutine(ApplyPowerUp(collector));
    }

    private IEnumerator ApplyPowerUp(PlayerController collector)
    {
        var movementData = collector.GetComponent<PlayerController>().MovementData;
        if (movementData != null)
        {
            float originalWalkSpeed = movementData.walkSpeed;
            float originalRunSpeed = movementData.runSpeed;
            
            movementData.walkSpeed *= speedBoostAmount;
            movementData.runSpeed *= speedBoostAmount;
            
            yield return new WaitForSeconds(duration);
            
            if (movementData != null)
            {
                movementData.walkSpeed = originalWalkSpeed;
                movementData.runSpeed = originalRunSpeed;
            }
        }
        powerUpCoroutine = null;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        powerUpCoroutine = null;
    }
}
