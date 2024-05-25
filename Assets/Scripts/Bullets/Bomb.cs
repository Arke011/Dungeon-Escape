using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody2D rb;
    private Health hp;
    private bool hitTarget;
    [SerializeField] private TrailRenderer rr;
    public GameObject blood;
    public GameObject explosionVFX;
    private AudioSource source;
    private bool hit;
    public float explosionRadius = 1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rr.emitting = true;
        source = GetComponent<AudioSource>();
    }

    void Start()
    {
        hitTarget = false;
        hit = false;
    }

    public void Initialize(Vector3 direction, float speed)
    {
        rb.velocity = direction.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("bullet") && !hit)
        {
            hit = true;
            rr.emitting = false;

            if (collision.contactCount > 0)
            {
                ContactPoint2D contact = collision.GetContact(0);
                GameObject boom = Instantiate(explosionVFX, contact.point, Quaternion.identity);
                StartCoroutine(DestroyVFX(boom));
            }

            source.Play();
            StartCoroutine(DestroyBullet());
        }

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("bullet"))
        {
            // Handle player or bullet collision
        }

        if (collision.gameObject.CompareTag("Enemy") && !hitTarget)
        {
            
            var inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D collider in inExplosionRadius)
            {
                var enemy = collider.GetComponent<Health>();

                if (enemy)
                {
                    var closestPoint = collider.ClosestPoint(transform.position);
                    var distance = Vector3.Distance(closestPoint, transform.position);
                    var damagePercent = Mathf.InverseLerp(explosionRadius, 0, distance);
                    enemy.TakeDamage(damagePercent * damage);
                }
            }

            if (collision.contactCount > 0)
            {
                ContactPoint2D contact = collision.GetContact(0);
                GameObject red = Instantiate(blood, contact.point, Quaternion.identity);
                GameObject kaboom = Instantiate(explosionVFX, contact.point, Quaternion.identity);
                StartCoroutine(DestroyVFX(kaboom));
                Destroy(red, 1f);
            }

            hitTarget = true;
            source.Play();
            StartCoroutine(DestroyBullet());
        }
    }

    private IEnumerator DestroyVFX(GameObject vfx)
    {
        yield return new WaitForSeconds(1f);
        Destroy(vfx);
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    
}
