using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject item;
    private Transform player;

    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
        
    }

    public void SpawnDropItem()
    {
        Vector2 playerPos = new Vector2(player.position.x, player.position.y + 1);
        Instantiate(item, playerPos, Quaternion.identity);
    }
}
