using UnityEngine;

public class PlayerCombat
{
    private PlayerController controller;
    private const float bounceForce = 8f;

    public PlayerCombat(PlayerController controller)
    {
        this.controller = controller;
    }

    public void Bounce()
    {
        var rb = controller.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
    }
}
