using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public bool canSee = false;
    [SerializeField]
    private float range;
    [SerializeField]
    private float maxDistance;

    private Vector2 patrolCenter;
    public Vector2 wayPoint;

    private Vector3 lastPosition;

    void Start()
    {
        patrolCenter = transform.position;
        SetNewDestination();
        lastPosition = transform.position;
    }

    void Update()
    {
        Vector2 targetPosition = canSee ? (Vector2)player.position : wayPoint;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (!canSee && Vector2.Distance(transform.position, wayPoint) < range)
        {
            SetNewDestination();
        }

        FlipTowardsMovementDirection();
    }

    void SetNewDestination()
    {
        wayPoint = patrolCenter + new Vector2(Random.Range(-maxDistance, maxDistance), Random.Range(-maxDistance, maxDistance));
    }

    private void FixedUpdate()
    {
        RaycastHit2D ray = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        if (ray.collider != null)
        {
            canSee = ray.collider.CompareTag("Player");
            if (canSee)
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.green);
            }
            else
            {
                Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red);
            }
        }
    }

    void FlipTowardsMovementDirection()
    {
        Vector3 currentPosition = transform.position;
        Vector3 movementDirection = currentPosition - lastPosition;

        if (movementDirection.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (movementDirection.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }

        lastPosition = currentPosition;
    }
}
