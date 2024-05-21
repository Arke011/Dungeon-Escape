using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    private Button button;
    private GunManager manager;
    private bool equipped;

    void Start()
    {
        equipped = false;
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }

        manager = FindObjectOfType<GunManager>();
        if (manager == null)
        {
            Debug.LogError("GunManager not found in the scene!");
        }
    }

    void OnButtonClick()
    {
        // Get the gun name from the button's name
        string gunName = gameObject.name;

        // Equip the correct gun based on the button's name
        switch (gunName)
        {
            case "pistolBtn(Clone)":
                if (manager != null && !equipped)
                {
                    manager.EquipGun1();
                    manager.UnequipGun2();
                    manager.UnequipGun3();
                    Debug.Log("Pistol equipped");
                    equipped = true;
                }
                else
                {
                    manager.UnequipGun1();
                    equipped = false;
                }
                break;

            case "shottyBtn(Clone)":
                if (manager != null && !equipped)
                {
                    manager.EquipGun2();
                    manager.UnequipGun3();
                    manager.UnequipGun1();
                    Debug.Log("Shotgun equipped");
                    equipped = true;
                }
                else
                {
                    manager.UnequipGun2();
                    equipped = false;
                }
                break;

            case "boomerBtn(Clone)":
                if (manager != null && !equipped)
                {
                    manager.EquipGun3();
                    manager.UnequipGun2();
                    manager.UnequipGun1();
                    Debug.Log("boomerang equipped");
                    equipped = true;
                }
                else
                {
                    manager.UnequipGun3();
                    equipped = false;
                }
                break;

            
            default:
                Debug.LogWarning("Unknown button clicked");
                break;
        }
    }
}
