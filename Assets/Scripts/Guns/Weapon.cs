using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform weaponPivot;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public WeaponPick pick;
    public float shootDelay = 0.5f; 
    public bool isShotgun;
    public GameObject boomerVisual;
    private bool isShooting;
    private bool canShoot = true;
    public bool isBoomer;
    public GameObject shootParticles;
    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (pick.gunInHand && this.transform.parent != null)
        {
            Aim();

            if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot)
            {
                StartCoroutine(Shoot());
            }
        }
    }

    private IEnumerator Shoot()
    {
        source.Play();
        if (isBoomer && !isShotgun)
        {
            canShoot = false;

            
            GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Bullet bullet = newBullet.GetComponent<Bullet>();

            if (bullet != null)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos.z = 0;

                Vector3 shootDirection = (mousePos - shootPoint.position).normalized;
                bullet.Initialize(shootDirection, bulletSpeed);
            }

            StartCoroutine(BulletGone(newBullet));

            boomerVisual.SetActive(false);
            yield return new WaitForSeconds(shootDelay);
            boomerVisual.SetActive(true);
            canShoot = true; //shooting cooldown :)))))
        }
        if (!isBoomer && !isShotgun)
        {
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

            StartCoroutine(BulletGone(newBullet));

            
            yield return new WaitForSeconds(shootDelay);
            
            canShoot = true; //shooting cooldown :)))))
        }
        if (isShotgun)
        {
            canShoot = false;

            
            int numBullets = 5;

            for (int i = 0; i < numBullets; i++)
            {
                
                float angleOffset = Random.Range(-5f, 5f);

                Instantiate(shootParticles, shootPoint.position, shootPoint.rotation);
                GameObject newBullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
                Bullet bullet = newBullet.GetComponent<Bullet>();

                if (bullet != null)
                {
                    
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mousePos.z = 0;
                    Vector3 shootDirection = Quaternion.Euler(0, 0, angleOffset) * (mousePos - shootPoint.position).normalized;

                    bullet.Initialize(shootDirection, bulletSpeed);
                }

                StartCoroutine(BulletGone(newBullet));
            }

            
            yield return new WaitForSeconds(shootDelay);

            canShoot = true; //shoot cooldown :DD
        } 
    }

    void Aim()
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

    private IEnumerator BulletGone(GameObject bullet)
    {
        yield return new WaitForSeconds(5f);
        Destroy(bullet);
    }
}
