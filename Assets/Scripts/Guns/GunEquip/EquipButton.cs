using UnityEngine;
using UnityEngine.UI;

public class EquipButton : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.Alpha1;

    private Button button;
    private GunManager manager;
    

    void Start()
    {
        
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

        if (manager != null)
        {
            switch (gunName)
            {
                case "pistolBtn(Clone)":
                    manager.EquipGun1();
                    break;

                case "shottyBtn(Clone)":
                    manager.EquipGun2();
                    break;

                case "boomerBtn(Clone)":
                    manager.EquipGun3();
                    break;

                case "bombBtn(Clone)":
                    manager.EquipBomb();
                    Destroy(gameObject);
                    break;

                default:
                    Debug.LogWarning("Unknown button clicked");
                    break;
            }
        }
        
    }
}
