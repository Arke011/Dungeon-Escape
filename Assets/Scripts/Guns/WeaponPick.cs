using UnityEngine;

public class WeaponPick : MonoBehaviour
{
    public Transform hand;
    public GameObject weapon;
    public bool gunInHand;
    private Weapon gun;

    void Start()
    {
        gunInHand = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && weapon != null)
        {
            if (gunInHand)
            {
                Drop();
            }
            else
            {
                Equip();
            }
        }
    }

    private void Equip()
    {
        if (!gunInHand)
        {
            gunInHand = true;
            weapon.transform.position = hand.position;
            weapon.transform.rotation = hand.rotation;
            weapon.transform.parent = hand;
            weapon.SetActive(true);

            gun = weapon.GetComponent<Weapon>();
            if (gun != null)
            {
                gun.pick = this;
            }
        }
    }

    private void Drop()
    {
        if (weapon == null) return;

        gunInHand = false;
        weapon.transform.parent = null;
        weapon.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            weapon = other.gameObject;
            weapon.SetActive(false);
            weapon.transform.position = hand.position;
            weapon.transform.rotation = hand.rotation;
            weapon.transform.parent = hand;
        }
    }

}
