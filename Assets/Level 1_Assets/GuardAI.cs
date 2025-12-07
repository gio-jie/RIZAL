using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardAI : MonoBehaviour
{
    [Header("Settings")]
    public float walkSpeed = 1f;
    public float chaseSpeed = 2f;
    public float visionRange = 6f;
    [Range(0, 360)] public float viewAngle = 90f;

    [Header("References")]
    public Transform[] patrolPoints; // Drag the path points here
    public LayerMask obstacleLayer;  // Set this to "Walls"

    private int currentPointIndex = 0;
    private Transform player;
    private bool isChasing = false;

    void Start()
    {
        // Find Rizal automatically
        // Note: Make sure your player has the RizalMovement script attached!
        RizalMovement playerScript = FindFirstObjectByType<RizalMovement>();
        if (playerScript != null)
        {
            player = playerScript.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // 1. Check Vision
        if (CanSeePlayer())
        {
            isChasing = true;
        }
        else
        {
            // Stop chasing if player gets too far away (Lost him)
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance > visionRange * 1.5f)
            {
                isChasing = false;
            }
        }

        // 2. Move based on state
        if (isChasing)
        {
            ChaseBehavior();
        }
        else
        {
            PatrolBehavior();
        }
    }

    bool CanSeePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // 1. Is he close enough?
        if (distanceToPlayer < visionRange)
        {
            // Calculate direction to player
            Vector2 dirToPlayer = (player.position - transform.position).normalized;

            // 2. Is he within the View Angle? (The Cone)
            // We use transform.up because in 2D Top-Down, 'Up' is usually 'Forward'
            if (Vector2.Angle(transform.up, dirToPlayer) < viewAngle / 2f)
            {
                // 3. Is there a wall in the way?
                if (!Physics2D.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleLayer))
                {
                    return true; // SEEN!
                }
            }
        }
        return false;
    }

    void RotateTowards(Vector3 targetPosition)
    {
        // Calculate direction
        Vector3 vectorToTarget = targetPosition - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

        // Apply rotation (offset by -90 because 2D sprites usually face Up)
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 10f);
    }

    void PatrolBehavior()
    {
        if (patrolPoints.Length == 0) return;

        Transform target = patrolPoints[currentPointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, walkSpeed * Time.deltaTime);

        RotateTowards(target.position);

        // Switch to next point when close
        if (Vector2.Distance(transform.position, target.position) < 0.2f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }

    void ChaseBehavior()
    {
        RotateTowards(player.position);
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // If Guard touches Rizal
        if (collision.gameObject.GetComponent<RizalMovement>())
        {
            Debug.Log("CAUGHT BY GUARD!");
            
            LevelManager manager = FindFirstObjectByType<LevelManager>();
            if (manager != null)
            {
                manager.GameOver();
            }
        }
    }
}