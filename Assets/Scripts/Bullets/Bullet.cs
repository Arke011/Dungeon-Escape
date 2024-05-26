using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody2D rb;
    Health hp;
    private bool hitTarget;
    [SerializeField] private TrailRenderer rr;
    public bool boomShot;
    public GameObject blood;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        hitTarget = false;
        rr.emitting = true;
    }

    void Start()
    {
        StartCoroutine(DestroyBullet());
    }

    public void Initialize(Vector3 direction, float speed)
    {
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("bullet"))
        {
            rr.emitting = false;
            ContactPoint2D contact = collision.GetContact(0);

        }

        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("enemyBullet"))
        {
            Player pl = collision.gameObject.GetComponent<Player>();
            pl.TakeDamage(damage);
            ContactPoint2D contact = collision.GetContact(0);
            GameObject red = Instantiate(blood, contact.point, Quaternion.identity);
            Destroy(red, 1f);
        }

        if (collision.gameObject.CompareTag("Enemy") && !hitTarget)
        {

            ContactPoint2D contact = collision.GetContact(0);
            GameObject red = Instantiate(blood, contact.point, Quaternion.identity);
            //print("hit enemy!");
            hp = collision.gameObject.GetComponent<Health>();
            hp.TakeDamage(damage);
            hitTarget = true;
            Destroy(red, 1f);

        }
    }

    private IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(0.5f);
        if (!boomShot)
        {
            Destroy(gameObject);
        }
    }
}
