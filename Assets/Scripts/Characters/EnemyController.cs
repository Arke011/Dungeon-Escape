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

    void Start()
    {
        patrolCenter = transform.position;
        SetNewDestination();
    }

    void Update()
    {
        if (canSee)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, wayPoint) < range)
            {
                SetNewDestination();
            }
        }
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
}
