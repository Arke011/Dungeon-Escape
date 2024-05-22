using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    private Inventory inv;
    public GameObject itemBtn;

    // Start is called before the first frame update
    void Start()
    {
        inv = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < inv.slots.Length; i++)
            {
                if (inv.isFull[i] == false)
                {
                    inv.isFull[i] = true;
                    GameObject newItemBtn = Instantiate(itemBtn, inv.slots[i].transform);
                    newItemBtn.transform.localPosition = Vector3.zero;

                    
                    EquipButton equipButton = newItemBtn.GetComponent<EquipButton>();
                    if (equipButton != null)
                    {
                        equipButton.activationKey = GetKeyCodeForSlot(i);
                    }

                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

    KeyCode GetKeyCodeForSlot(int slotIndex)
    {
        switch (slotIndex)
        {
            case 0: return KeyCode.Alpha1;
            case 1: return KeyCode.Alpha2;
            case 2: return KeyCode.Alpha3;
            default: return KeyCode.None;
        }
    }
}
