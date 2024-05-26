using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public float speed = 5f;
    public float attackCD;
    
    [Space]
    public GameObject enemyPrefab;
    public Transform[] enemySpawnPos;
    public float enemySpawnCD;
    [Space]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject slicePrefab;
    public Transform shootTarget;
    public float bulletSpeed;
    public float shootCD;
    [Space]
    public float rotationSpeed;
    public AudioClip stageTwoSound;
    public AudioSource audioSource;

    private float attackTimer;
    private float shootTimer;
    private float enemySpawnTimer;
    private Transform target;

    private Animator anim;
    private bool isAttackingPlayer = false;
    private bool isShooting = false;
    private bool isEnemySpawning = false;
    private bool facingRight = true;

    public float damage;

    private enum BossStage { Stage1, Stage2 }
    private BossStage currentStage = BossStage.Stage1;

    Health hp;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player")?.transform;
        attackTimer = attackCD;
        shootTimer = shootCD;
        enemySpawnTimer = enemySpawnCD;
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        hp = GetComponent<Health>();
    }

    private void Update()
    {
        enemySpawnTimer -= Time.deltaTime;
        shootTimer -= Time.deltaTime;
        attackTimer -= Time.deltaTime;

        if (target != null)
        {
            FlipTowardsPlayer();
        }

        switch (currentStage)
        {
            case BossStage.Stage1:
                HandleStage1();
                break;
            case BossStage.Stage2:
                HandleStage2();
                break;
        }

        anim.SetBool("isAttacking", isAttackingPlayer);
        anim.SetBool("isSummoning", isEnemySpawning);
        anim.SetBool("isShooting", isShooting);

        if (hp.currentHealth <= hp.maxHealth / 2 && currentStage == BossStage.Stage1)
        {
            TransitionToStage2();
        }
    }

    private void HandleStage1()
    {
        enemySpawnCD = 4f;
        if (shootTimer <= 0)
        {
            Shoot1();
            shootTimer = shootCD;
        }
        if (enemySpawnTimer <= 0)
        {
            SpawnEnemy();
            enemySpawnTimer = enemySpawnCD;
        }
    }

    private void HandleStage2()
    {
        enemySpawnCD = 1f;
        if (target != null)
        {
            Debug.Log("Boss is moving towards the player.");
            Vector3 direction = target.position - transform.position;
            direction.Normalize();
            transform.Translate(direction * speed * Time.deltaTime);
        }

        if (shootTimer <= 0)
        {
            Shoot2();
            shootTimer = shootCD;
        }

        if (enemySpawnTimer <= 0)
        {
            SpawnEnemy();
            enemySpawnTimer = enemySpawnCD;
        }
    }

    private void TransitionToStage2()
    {
        Debug.Log("Transitioning to Stage 2");
        currentStage = BossStage.Stage2;
        audioSource.PlayOneShot(stageTwoSound);
        hp.IncreaseHealth(1000f);
    }

    private void Shoot1()
    {
        if (target != null)
        {
            isAttackingPlayer = true;
            GameObject bullet = Instantiate(slicePrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (target.position - firePoint.position).normalized;

            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

            StartCoroutine(ResetBool());
        }
    }


    private void Shoot2()
    {
        if (target != null)
        {
            isShooting = true;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (target.position - firePoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            StartCoroutine(ResetBool());
        }
    }

    private void SpawnEnemy()
    {
        isEnemySpawning = true;
        int randomSpawnPoint = Random.Range(0, enemySpawnPos.Length);
        Instantiate(enemyPrefab, enemySpawnPos[randomSpawnPoint].position, Quaternion.identity);
        StartCoroutine(ResetBool());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            print("boss hit");
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            attackTimer = attackCD;
            collision.gameObject.GetComponent<Player>().TakeDamage(damage);
            isAttackingPlayer = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackTimer <= 0)
        {
            attackTimer = attackCD;
            collision.gameObject.GetComponent<Health>().TakeDamage(damage);
            isAttackingPlayer = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttackingPlayer = false;
        }
    }

    private void FlipTowardsPlayer()
    {
        if (target == null) return;

        Vector3 directionToPlayer = target.position - transform.position;

        if ((directionToPlayer.x > 0 && !facingRight) || (directionToPlayer.x < 0 && facingRight))
        {
            facingRight = !facingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private IEnumerator ResetBool()
    {
        yield return new WaitForSeconds(0.75f);
        isShooting = false;
        isEnemySpawning = false;
        isAttackingPlayer = false;
    }
}
