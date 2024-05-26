using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BetterGun : MonoBehaviour
{
    public Transform weaponPivot;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootDelay;
    public GameObject shootParticles;
    private AudioSource source;
    private bool canShoot = true;
    public bool isBoomer;
    public bool isShotgun;
    public GameObject boomerVisual;
    public float coolDownTimer;
    private bool isIT;
    public bool isBomb;

    
    

    void Start()
    {
        source = GetComponent<AudioSource>();
        weaponPivot = transform.parent.parent;
        transform.rotation = weaponPivot.rotation;
        shootPoint = GameObject.Find("shootPoint").transform;
        canShoot = false;
        StartCooldown();
    }

    void Update()
    {
        if (shootDelay > 0)
        {
            shootDelay -= Time.deltaTime;
            if (shootDelay <= 0)
            {
                canShoot = true;
                
                if (isBoomer)
                {
                    boomerVisual.GetComponent<SpriteRenderer>().enabled = true;
                }
            }
        }

        Aim();

        if (!isIT && Input.GetKey(KeyCode.Mouse0) && canShoot)
        {
            Shoot();
            StartCooldown();
        }

        isIT = EventSystem.current.IsPointerOverGameObject();
        
    }

    private void Shoot()
    {
        if (!isBoomer && !isShotgun && !isBomb)
        {
            source.Play();
            canShoot = false;

            Instantiate(shootParticles, shootPoint.position, shootPoint.rotation);
            GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            CinemachineShake.Instance.ShakeCamera(1f, 0.1f);
            Bullet bullet = newBullet.GetComponent<Bullet>();

            if (bullet != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                Vector3 shootDirection = (mousePos - shootPoint.position).normalized;
                bullet.Initialize(shootDirection, bulletSpeed);
            }



        }
        if (isShotgun)
        {
            source.Play();
            canShoot = false;
            int numBullets = 5;
            CinemachineShake.Instance.ShakeCamera(2f, 0.2f);
            for (int i = 0; i < numBullets; i++)
            { 
                float angleOffset = Random.Range(-2f, 2f);
    
                GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                Bullet bullet = newBullet.GetComponent<Bullet>();

                if (bullet != null)
                { 
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;
                    Vector3 shootDirection = Quaternion.Euler(0, 0, angleOffset) * (mousePos - shootPoint.position).normalized;

                    bullet.Initialize(shootDirection, bulletSpeed);
                }
                
            }


            
        }
        
        if (isBoomer)
        {
            source.Play();
            canShoot = false;

            Instantiate(shootParticles, shootPoint.position, shootPoint.rotation);
            GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Bullet bullet = newBullet.GetComponent<Bullet>();

            if (bullet != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                Vector3 shootDirection = (mousePos - shootPoint.position).normalized;
                bullet.Initialize(shootDirection, bulletSpeed);
            }

            
            boomerVisual.GetComponent<SpriteRenderer>().enabled = false;
               
        }
        if (isBomb)
        {
            Destroy(gameObject);
            canShoot = false;

            

            Instantiate(shootParticles, shootPoint.position, shootPoint.rotation);
            GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Bomb bullet = newBullet.GetComponent<Bomb>();

            if (bullet != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                Vector3 shootDirection = (mousePos - shootPoint.position).normalized;
                bullet.Initialize(shootDirection, bulletSpeed);
            }

            
        }
    }

    private void Aim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = mousePosition - weaponPivot.position;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

        if (mouseDirection.x < 0)
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

    private void StartCooldown()
    {
        shootDelay = coolDownTimer;
    }






}
