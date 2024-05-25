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

    
    

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;
        isChasing = false;
        startAttackCD = attackCD;
      
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
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            FlipTowards(waypoints[currentWaypointIndex].position);
        }

        if (chaseTimer > 0f)
        {
            chaseTimer -= Time.deltaTime;
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
            //hp.TakeDamage(20);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            
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
