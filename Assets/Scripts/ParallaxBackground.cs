using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float parallaxEffect = 0.5f;
    [SerializeField] private bool infiniteHorizontal = true;
    [SerializeField] private SpriteRenderer[] backgroundSprites;
    
    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float spriteWidth;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        
        if (backgroundSprites.Length > 0)
        {
            spriteWidth = backgroundSprites[0].bounds.size.x;
        }
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0f, 0f);
        lastCameraPosition = cameraTransform.position;

        if (infiniteHorizontal && backgroundSprites != null)
        {
            foreach (var sprite in backgroundSprites)
            {
                if (sprite == null) continue;
                
                float distanceFromCamera = (cameraTransform.position.x - sprite.transform.position.x);
                float totalWidth = spriteWidth * backgroundSprites.Length;
                
                if (distanceFromCamera > totalWidth / 2)
                {
                    sprite.transform.position += new Vector3(totalWidth, 0, 0);
                }
                else if (distanceFromCamera < -totalWidth / 2)
                {
                    sprite.transform.position -= new Vector3(totalWidth, 0, 0);
                }
            }
        }
    }
}
