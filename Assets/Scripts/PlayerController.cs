using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool isHidden = false; // Status kung nagtatago si Rizal
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Simple movement (WASD or Arrow Keys)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(moveX, 0, moveZ).normalized;

        // Move the player
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);

        // Rotate player to face direction
        if (movement != Vector3.zero)
        {
            transform.forward = movement;
        }

        // Hiding Mechanic (Press Space near furniture)
        if (Input.GetKeyDown(KeyCode.Space) && isHidden)
        {
            // Logic para lumabas sa tago, mamaya dadagdagan natin to
            Debug.Log("Trying to hide/unhide");
        }
    }
}