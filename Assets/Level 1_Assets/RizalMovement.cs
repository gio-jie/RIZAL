using UnityEngine;

public class RizalMovement : MonoBehaviour
{
    public float speed = 5f;
    public VirtualJoystick joystick;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (joystick == null) return;

        Vector2 moveInput = joystick.InputDirection;

        if (moveInput != Vector2.zero)
        {
            // 1. Move
            rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);

            // 2. Rotate
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90f;

            // 3. Animate
            anim.SetBool("isRunning", true);
        }
        else
        {
            // --- MOVEMENT STOPPED ---
            anim.SetBool("isRunning", false);

            // KILL SPINNING MOMENTUM
            rb.angularVelocity = 0f;
        }
    }
}