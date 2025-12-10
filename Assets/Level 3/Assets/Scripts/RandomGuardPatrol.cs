using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class RandomGuardPatrol : MonoBehaviour
{
    [Header("PATROL SETTINGS")]
    public List<Transform> patrolPoints; // Assign waypoints in inspector
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 1f;

    [Header("CHASE SETTINGS")]
    public Transform player;
    public float chaseSpeed = 4f;
    public bool isChasing = false;

    [Header("VISION SETTINGS")]
    public FieldOfView fov;
    public LayerMask obstacleMask;
    public bool canDetectPlayer = true;
    public float loseSightTime = 2f;

    private Rigidbody2D rb;
    public int currentPointIndex = 0;
    private float loseSightTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (patrolPoints.Count > 0)
        {
            // Spawn on a random waypoint
            currentPointIndex = Random.Range(0, patrolPoints.Count);
            transform.position = patrolPoints[currentPointIndex].position;
            StartCoroutine(PatrolRoutine());
        }

        if (fov != null)
            obstacleMask = fov.obstacleMask;
    }

    void FixedUpdate()
    {
        if (isChasing)
        {
            ChasePlayer();
            DetectPlayer();
        }
        else
        {
            DetectPlayer(); // Keep checking for player
        }
    }

    IEnumerator PatrolRoutine()
    {
        while (!isChasing)
        {
            Transform target = patrolPoints[currentPointIndex];

            // Move toward the target
            while (Vector2.Distance(rb.position, target.position) > 0.05f && !isChasing)
            {
                rb.MovePosition(Vector2.MoveTowards(rb.position, target.position, patrolSpeed * Time.fixedDeltaTime));

                // Rotate guard to face movement direction
                Vector2 dir = (target.position - transform.position).normalized;
                if (dir.sqrMagnitude > 0.001f)
                {
                    float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
                }

                yield return new WaitForFixedUpdate();
            }

            // Wait at the waypoint
            yield return new WaitForSeconds(waitTimeAtPoint);

            // Move to next waypoint
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        }
    }

    void ChasePlayer()
    {
        if (player == null) return;

        rb.MovePosition(Vector2.MoveTowards(rb.position, player.position, chaseSpeed * Time.fixedDeltaTime));

        Vector2 dir = (player.position - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    void DetectPlayer()
    {
        if (!canDetectPlayer || player == null || fov == null) return;

        Vector2 dirToPlayer = player.position - transform.position;
        float dist = dirToPlayer.magnitude;

        // Outside view radius or angle
        if (dist > fov.viewRadius || Vector2.Angle(transform.up, dirToPlayer.normalized) > fov.viewAngle / 2f)
        {
            LoseSight();
            return;
        }

        // Obstacle check
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dirToPlayer.normalized, dist, obstacleMask);
        if (hit.collider != null)
        {
            LoseSight();
            return;
        }

        // Player detected
        if (!isChasing)
        {
            isChasing = true;
            loseSightTimer = 0f;
            StopCoroutine(PatrolRoutine());
        }
    }

    void LoseSight()
    {
        if (!isChasing) return;

        loseSightTimer += Time.fixedDeltaTime;
        if (loseSightTimer >= loseSightTime)
        {
            isChasing = false;
            loseSightTimer = 0f;
            StartCoroutine(PatrolRoutine());
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Player"))
        {
            Debug.Log("GAME OVER!");
            Time.timeScale = 0;
        }
    }
}
