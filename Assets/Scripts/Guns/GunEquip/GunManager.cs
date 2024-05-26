using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject activeGun1;
    public GameObject activeGun2;
    public GameObject activeGun3;
    public GameObject bombPrefab;
    public Transform weaponPivot;
    public Transform weaponOffset;
    GameObject explode;
    GameObject gun1;
    GameObject gun2;
    GameObject gun3;
    public GameObject flashlight;
    //bool equipped;
    
    void Start()
    {
        //equipped = false;
    }

    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 mouseDirection = mousePosition - weaponPivot.position;
        float angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

        if (mouseDirection.x < 0)
        {
            weaponOffset.localScale = new Vector3(-1, 1, 1);
            angle += 180f;
        }
        else
        {
            weaponOffset.localScale = new Vector3(1, 1, 1);
        }
        weaponOffset.eulerAngles = new Vector3(0, 0, angle);
    }



    public void EquipGun1()
    {
        UnequipAll();

        gun1 = Instantiate(activeGun1, weaponPivot.position, weaponPivot.rotation);

        gun1.transform.SetParent(weaponPivot, false);

        gun1.transform.localPosition = Vector3.zero;
        gun1.transform.localRotation = Quaternion.identity;

        UnequipGun2();
        UnequipGun3();
        UnequipBomb();
        
    }

    public void EquipGun2()
    {
        UnequipAll();

        gun2 = Instantiate(activeGun2, weaponPivot.position, weaponPivot.rotation);

        gun2.transform.SetParent(weaponPivot, false);

        gun2.transform.localPosition = Vector3.zero;
        gun2.transform.localRotation = Quaternion.identity;

        UnequipGun1();
        UnequipGun3();
        UnequipBomb();
        
    }

    public void EquipGun3()
    {
        UnequipAll();

        gun3 = Instantiate(activeGun3, weaponPivot.position, weaponPivot.rotation);

        gun3.transform.SetParent(weaponPivot, false);

        gun3.transform.localPosition = Vector3.zero;
        gun3.transform.localRotation = Quaternion.identity;

        UnequipGun2();
        UnequipGun1();
        UnequipBomb();
        
    }

    public void EquipBomb()
    {
        
        UnequipAll();
        explode = Instantiate(bombPrefab, weaponPivot.position, weaponPivot.rotation);

        explode.transform.SetParent(weaponPivot, false);

        explode.transform.localPosition = Vector3.zero;
        explode.transform.localRotation = Quaternion.identity;

        UnequipGun1();
        UnequipGun2();
        UnequipGun3();
        
    }


    public void UnequipGun1()
    {
        Destroy(gun1);
        
    }

    public void UnequipGun2()
    {
        Destroy(gun2);
        
    }

    public void UnequipGun3()
    {
        Destroy(gun3);
        
    }

    public void UnequipBomb()
    {
        Destroy(explode);
        //quipped = false;
        
    }

    public void UnequipAll()
    {
        Destroy(gun1);
        Destroy(gun2);
        Destroy(gun3);
        Destroy(explode);
        
    }
}
