using UnityEngine;

[RequireComponent(typeof(Item))]
public class ItemFloating : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float amplitude = 0.5f;    // Altura da flutuação
    [SerializeField] private float frequency = 1f;      // Velocidade da flutuação
    [SerializeField] private float verticalOffset = 0f; // Offset vertical inicial

    private Vector3 startPosition;
    private float sinTime;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        sinTime += Time.deltaTime * frequency;
        
        float yOffset = amplitude * Mathf.Sin(sinTime) + verticalOffset;
        transform.position = startPosition + Vector3.up * yOffset;
    }

    private void OnDisable()
    {
        // Reseta a posição quando o objeto é desativado
        transform.position = startPosition;
        sinTime = 0f;
    }
}
