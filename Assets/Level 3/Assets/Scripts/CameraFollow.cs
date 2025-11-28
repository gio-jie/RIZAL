using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // Player
    public float followSpeed = 5f; // Smoothness

    private void LateUpdate()
    {
        if (target == null) return;

        // Follow only X and Y
        Vector3 newPos = new Vector3(
            target.position.x,
            target.position.y,
            transform.position.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            newPos,
            followSpeed * Time.deltaTime
        );
    }
}
