using UnityEngine;

public class FollowRizalCamera : MonoBehaviour {
    public Transform target;       // This will be Rizal
    public float smoothSpeed = 0.125f; // Higher = faster lock, Lower = smoother delay
    public Vector3 offset;         // The distance between camera and player (usually Z = -10)

    void LateUpdate()
    {
        // Safety check
        if (target == null) return;

        // 1. Calculate where the camera *should* be
        // We add the offset so the camera stays Z: -10 away
        Vector3 desiredPosition = target.position + offset;

        // 2. Smoothly move from current position to desired position
        // This creates that nice "cinematic" delay feeling
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 3. Apply the position
        transform.position = smoothedPosition;
    }
}