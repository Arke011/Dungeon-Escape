using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float damage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Debug.Log("Player damaged");
                collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
