using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Range(0.1f, 10f)] private float smoothSpeed = 5f;
    [SerializeField, Range(0.5f, 5f)] private float maxDistance = 2f;
    [SerializeField] private Vector3 offset = new Vector3(0, 1, -10);

    private Vector3 velocity = Vector3.zero;
    private Camera mainCamera;
    private bool isInitialized;

    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        mainCamera = GetComponent<Camera>();
        isInitialized = target != null && mainCamera != null;

        if (!isInitialized)
        {
            Debug.LogError($"[{nameof(CameraFollow)}] Missing required components!");
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (!isInitialized) return;
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 desiredPosition = CalculateDesiredPosition();
        desiredPosition = ClampDistanceToTarget(desiredPosition);
        
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            1f / smoothSpeed
        );
    }

    private Vector3 CalculateDesiredPosition()
    {
        return new Vector3(
            target.position.x + offset.x,
            transform.position.y,
            transform.position.z
        );
    }

    private Vector3 ClampDistanceToTarget(Vector3 position)
    {
        float distanceToTarget = Mathf.Abs(position.x - target.position.x);
        
        if (distanceToTarget > maxDistance)
        {
            float direction = Mathf.Sign(target.position.x - transform.position.x);
            position.x = target.position.x - (maxDistance * direction);
        }

        return position;
    }
}
