using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public float damage;
    private Transform target;
    private bool isChasing;

    private float chaseTimer = 3f;
    public float speed;
    public float chaseRange = 5f;

    public float attackCD;
    float startAttackCD;
    public Health hp;

    public Transform[] waypoints;
    NavMeshAgent agent;
    int currentWaypointIndex;

    public bool isMonster;

    bool canSee;
    

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (target == null)
        {
            Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
            return;
        }

        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on this GameObject.");
            return;
        }

        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        isChasing = false;
        startAttackCD = attackCD;

        // Ensure waypoints are assigned and contain elements
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning("Waypoints not assigned or empty. The enemy will not patrol.");
            return;
        }

        StartCoroutine(MoveToNextWaypointEveryTwoSeconds());
    }

    IEnumerator MoveToNextWaypointEveryTwoSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            SetDestinationToNextWaypoint();
        }
    }

    void SetDestinationToNextWaypoint()
    {
        if (waypoints == null || waypoints.Length == 0)
        {
            return;
        }

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        if (target == null || agent == null)
        {
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= chaseRange)
        {
            isChasing = true;
            chaseTimer = 3f;
        }
        else
        {
            isChasing = false;
        }

        if (isChasing && chaseTimer > 0f)
        {
            agent.SetDestination(target.position);
            FlipTowards(target.position);
        }
        else
        {
            // Check if waypoints are assigned and not empty before using them
            if (waypoints != null && waypoints.Length > 0)
            {
                agent.SetDestination(waypoints[currentWaypointIndex].position);
                FlipTowards(waypoints[currentWaypointIndex].position);
            }
        }

        if (chaseTimer > 0f)
        {
            chaseTimer -= Time.deltaTime;
        }

        if (isMonster && distanceToTarget <= 4f && canSee == true)
        {
            agent.speed = 0f;
        }
        if (isMonster && !canSee)
        {
            agent.speed = speed;
        }

        attackCD -= Time.deltaTime;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackCD <= 0)
        {
            AttackPlayer(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer(collision.gameObject);
        }
    }

    private void AttackPlayer(GameObject player)
    {
        attackCD = startAttackCD;
        player.GetComponent<Player>().TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            // hp.TakeDamage(20);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle collision exit logic if needed
        }
    }

    private void FlipTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direction.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        RaycastHit2D ray = Physics2D.Raycast(transform.position, target.position - transform.position);
        if (ray.collider != null)
        {
            canSee = ray.collider.CompareTag("Player");
            if (canSee)
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
            }
        }
    }
}
