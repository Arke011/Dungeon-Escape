using UnityEngine;

public class GunManager : MonoBehaviour
{
    public GameObject activeGun1;
    public GameObject activeGun2;
    public GameObject activeGun3;

    public void EquipGun1()
    {
        activeGun1.SetActive(true);
        activeGun2.SetActive(false);
        activeGun3.SetActive(false);
    }

    public void EquipGun2()
    {
        activeGun1.SetActive(false);
        activeGun2.SetActive(true);
        activeGun3.SetActive(false);
    }

    public void EquipGun3()
    {
        activeGun1.SetActive(false);
        activeGun2.SetActive(false);
        activeGun3.SetActive(true);
    }

    public void UnequipGun1()
    {
        activeGun1.SetActive(false);
    }
    public void UnequipGun2()
    { 
        activeGun2.SetActive(false);
    }
    public void UnequipGun3()
    {
        activeGun3.SetActive(false);
    }
    public void UnequipAll()
    {
        activeGun1.SetActive(false);
        activeGun2.SetActive(false);
        activeGun3.SetActive(false);
    }
}
