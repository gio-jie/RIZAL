using UnityEngine;

public class RizalMovement : MonoBehaviour
{
    public float speed = 5f;

    // Reference to the Joystick script we made earlier
    public VirtualJoystick joystick;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        // Safety check: make sure joystick is assigned
        if (joystick == null) return;

        // 1. Get Input from Joystick script
        Vector2 moveInput = joystick.InputDirection;

        // 2. Move Rizal (the player) using Physics
        if (moveInput != Vector2.zero)
        {
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
        }
    }
}