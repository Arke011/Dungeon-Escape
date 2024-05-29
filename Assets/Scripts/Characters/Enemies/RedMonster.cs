using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedMonster : MonoBehaviour
{
    public Transform weaponPivot;
    Transform player;
    public float shootTimer = 0.5f;
    public Transform shootPoint;
    public float bulletSpeed;
    public GameObject shootParticles;
    public GameObject bulletPrefab;
    AudioSource source;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shootTimer = 1f;
        source = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        if (distanceToTarget <= 4f && shootTimer <= 0f)
        {
            Shoot(player);
        }
        if (distanceToTarget <= 8f)
        {
            AimAtTarget(player);
        }
        
        shootTimer -= Time.deltaTime;
    }

    private void AimAtTarget(Transform target)
    {
        Vector3 targetPosition = target.position;
        Vector3 directionToTarget = targetPosition - weaponPivot.position;
        float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

        if (directionToTarget.x < 0)
        {
            weaponPivot.localScale = new Vector3(-1, 1, 1);
            angle += 180f;
        }
        else
        {
            weaponPivot.localScale = new Vector3(1, 1, 1);
        }
        weaponPivot.eulerAngles = new Vector3(0, 0, angle);
    }

    void Shoot(Transform target)
    {
        source.Play();
        shootTimer = 0.5f;

        Instantiate(shootParticles, shootPoint.position, shootPoint.rotation);
        GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        Bullet bullet = newBullet.GetComponent<Bullet>();

        if (bullet != null)
        {
            Vector3 shootPos = target.position;

            Vector3 shootDirection = (target.position - shootPoint.position).normalized;
            bullet.Initialize(shootDirection, bulletSpeed);
        }
    }

}
