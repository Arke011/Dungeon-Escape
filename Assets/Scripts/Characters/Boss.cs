using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 5f;
    public float attackCD;
    public Health hp;
    [Space]
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPos;
    public float enemySpawnCD;


    private float startAttackCD;
    private float startEnemySpawnCD;
    private Transform target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        startAttackCD = attackCD;
        startEnemySpawnCD = enemySpawnCD;
    }

    private void Update()
    {
        enemySpawnCD -= Time.deltaTime;
        attackCD -= Time.deltaTime;
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }

        if (enemySpawnCD <= 0)
        {
            int randomSpawnPoint = Random.Range(0, enemySpawnPos.Length);
            Instantiate(enemyPrefab, enemySpawnPos[randomSpawnPoint].position, Quaternion.identity);
            enemySpawnCD = startEnemySpawnCD;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            print("a");
            hp.TakeDamage(20);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackCD <= 0)
        {
            attackCD = startAttackCD;
            collision.gameObject.GetComponent<Health>().TakeDamage(10);
        }
    }
}
