using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.Alpha1;

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

    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            button.onClick.Invoke();
        }
    }

    void OnButtonClick()
    {
        string gunName = gameObject.name;

        switch (gunName)
        {
            case "pistolBtn(Clone)":
                if (manager != null)
                {
                    manager.EquipGun1();
                    Debug.Log("Pistol equipped");
                    //equipped = true;
                }
                else
                {
                    manager.UnequipGun1();
                    //equipped = false;
                }
                break;

            case "shottyBtn(Clone)":
                if (manager != null)
                {
                    manager.EquipGun2();
                    Debug.Log("Shotgun equipped");
                    //equipped = true;
                }
                else
                {
                    manager.UnequipGun2();
                    //equipped = false;
                }
                break;

            case "boomerBtn(Clone)":
                if (manager != null)
                {
                    manager.EquipGun3();
                    Debug.Log("boomerang equipped");
                    //equipped = true;
                }
                else
                {
                    manager.UnequipGun3();
                    //equipped = false;
                }
                break;

            default:
                Debug.LogWarning("Unknown button clicked");
                break;
        }
    }
}
