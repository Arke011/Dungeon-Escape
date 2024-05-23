using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    private Transform target;
    private bool canSee;
    private bool isChasing;
    private float chaseTimer = 3f;
    public float viewRadius = 15f;
    public float speed;

    public Transform[] waypoints;

    NavMeshAgent agent;

    int currentWaypointIndex;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        canSee = false;
        agent.speed = speed;
        isChasing = false;

        StartCoroutine(MoveToNextWaypointEveryTwoSeconds());
    }

    IEnumerator MoveToNextWaypointEveryTwoSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f);
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
        }
        if (!canSee)
        {
            chaseTimer -= Time.deltaTime;
        }
        if (!isChasing)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
        if (chaseTimer <= 0f)
        {
            isChasing = false;
        }
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
}
