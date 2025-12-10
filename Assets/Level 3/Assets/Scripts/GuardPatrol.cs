using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class GuardPatrol : MonoBehaviour
{
    [Header("PATROL SETTINGS")]
    public Transform patrolPointA;
    public Transform patrolPointB;
    public float patrolSpeed = 2f;

    private Transform currentTarget;

    [Header("CHASE SETTINGS")]
    public float chaseSpeed = 4f;
    public Transform player;
    public bool isChasing = false;

    [Header("HIDE DETECTION")]
    public bool canDetectPlayer = true;

    [Tooltip("Seconds the guard will keep chasing after losing sight")]
    public float loseSightTime = 2f;
    float loseSightTimer = 0f;

    [Header("VISION (uses FieldOfView)")]
    public FieldOfView fov; // assign the FieldOfView component (child with MeshFilter)
    // Note: fov.viewRadius and fov.viewAngle are used
    public LayerMask obstacleMask; // same as fov.obstacleMask (for raycasts)

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = patrolPointA != null ? patrolPointA : transform;
        if (fov != null) obstacleMask = fov.obstacleMask;
    }

    void FixedUpdate()
    {
        if (!isChasing)
        {
            Patrol();
            DetectPlayer();
        }
        else
        {
            ChasePlayer();
            DetectPlayer(); // keep checking to possibly lose sight
        }
    }

    // ========================
    //       PATROL MODE
    // ========================
    void Patrol()
    {
        if (currentTarget == null) return;

        // Move to patrol target
        rb.MovePosition(Vector2.MoveTowards(
            rb.position,
            currentTarget.position,
            patrolSpeed * Time.fixedDeltaTime
        ));

        // Rotate guard to face movement direction
        Vector2 dir = (currentTarget.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }

        // If reaching patrol point, switch target and face the next
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.1f)
        {
            currentTarget = currentTarget == patrolPointA ? patrolPointB : patrolPointA;
            // Immediately rotate to face the next target
            Vector2 nextDir = (currentTarget.position - transform.position).normalized;
            if (nextDir.sqrMagnitude > 0.001f)
            {
                float nextAngle = Mathf.Atan2(nextDir.y, nextDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, nextAngle - 90f);
            }
        }
    }

    // ========================
    //       CHASE MODE
    // ========================
    void ChasePlayer()
    {
        if (player == null) return;

        Vector2 newPos = Vector2.MoveTowards(
            rb.position,
            player.position,
            chaseSpeed * Time.fixedDeltaTime
        );
        rb.MovePosition(newPos);

        // Rotate guard to face player
        Vector2 dir = (player.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    // ========================
    //     VISION + DETECTION
    // ========================
    void DetectPlayer()
    {
        if (!canDetectPlayer)
        {
            // If currently chasing but player hides -> stop chase
            if (isChasing)
            {
                isChasing = false;
                loseSightTimer = 0f;
            }
            return;
        }

        if (player == null || fov == null) return;

        Vector2 dirToPlayer = (player.position - transform.position);
        float dist = dirToPlayer.magnitude;

        // 1) distance check
        if (dist > fov.viewRadius)
        {
            // out of radius -> lose sight countdown if currently chasing
            if (isChasing)
            {
                loseSightTimer += Time.fixedDeltaTime;
                if (loseSightTimer >= loseSightTime)
                {
                    isChasing = false;
                    loseSightTimer = 0f;
                }
            }
            return;
        }

        // 2) angle check (use fov.viewAngle total)
        float angleToPlayer = Vector2.Angle(transform.up, dirToPlayer.normalized);
        if (angleToPlayer > fov.viewAngle / 2f)
        {
            // outside cone
            if (isChasing)
            {
                loseSightTimer += Time.fixedDeltaTime;
                if (loseSightTimer >= loseSightTime)
                {
                    isChasing = false;
                    loseSightTimer = 0f;
                }
            }
            return;
        }

        // 3) occlusion check - raycast toward player, see if obstacle in between
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, dist, obstacleMask);
        if (hit.collider != null)
        {
            // there's something blocking the view (wall)
            if (isChasing)
            {
                loseSightTimer += Time.fixedDeltaTime;
                if (loseSightTimer >= loseSightTime)
                {
                    isChasing = false;
                    loseSightTimer = 0f;
                }
            }
            return;
        }

        // Player visible (not blocked and within cone)
        isChasing = true;
        loseSightTimer = 0f;
    }

    // ========================
    //       GAME OVER
    // ========================
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            Debug.Log("GAME OVER!");
            Time.timeScale = 0;
        }
    }
}
