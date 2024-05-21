using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform weaponPivot;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;
    public float shootDelay = 0.5f;
    public GameObject shootParticles;
    private AudioSource source;
    private bool canShoot = true;
    public WeaponPick pick;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (pick != null && pick.gunInHand && this.transform.parent != null && this.gameObject.activeSelf)
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
        canShoot = true;
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

    private IEnumerator BulletGone(GameObject bullet)
    {
        yield return new WaitForSeconds(2f);
        Destroy(bullet);
    }
}
