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
                    Destroy(gameObject);
                    break;
                }
            }
        }
    }

}
