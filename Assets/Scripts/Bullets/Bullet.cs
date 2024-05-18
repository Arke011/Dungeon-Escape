using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    private Rigidbody2D rb;
    Health hp;
    private bool hitTarget;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
        hitTarget = false;
    }

    public void Initialize(Vector3 direction, float speed)
    {
        rb.velocity = new Vector2(direction.x, direction.y) * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !hitTarget)
        {
            print("hit enemy!");
            hp = collision.gameObject.GetComponent<Health>();
            hp.TakeDamage(damage);
            hitTarget = true;
        }
    }

    
}
