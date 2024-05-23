using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            if(collision.gameObject.CompareTag("Player"))
            {
                print("damaged");
                collision.gameObject.GetComponent<Health>().TakeDamage(30);
            }
            Destroy(gameObject);
        }
    }
}
