using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    private Transform target;
    private bool canSee;
    private bool isChasing;
    private bool isAttacking;
    private float chaseTimer = 3f;
    public float speed;

    public float attackCD;
    float startAttackCD;
    public Health hp;

    public Transform[] waypoints;
    NavMeshAgent agent;
    int currentWaypointIndex;
    Animator anim;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        canSee = false;
        agent.speed = speed;
        isChasing = false;
        startAttackCD = attackCD;
        anim = GetComponent<Animator>();
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
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Update()
    {
        if (canSee)
        {
            isChasing = true;
            chaseTimer = 3f;
        }
        if (isChasing && chaseTimer > 0f)
        {
            agent.SetDestination(target.position);
            FlipTowards(target.position);
        }
        if (!canSee)
        {
            chaseTimer -= Time.deltaTime;
        }
        if (!isChasing)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            FlipTowards(waypoints[currentWaypointIndex].position);
        }
        if (chaseTimer <= 0f)
        {
            isChasing = false;
        }
        attackCD -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackCD <= 0)
        {
            AttackPlayer(collision.gameObject);
        }
    }

    private void AttackPlayer(GameObject player)
    {
        attackCD = startAttackCD;
        player.GetComponent<Player>().TakeDamage(20);
        isAttacking = true;
        anim.SetBool("isAttacking", isAttacking);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("bullet"))
        {
            hp.TakeDamage(20);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);
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
}
