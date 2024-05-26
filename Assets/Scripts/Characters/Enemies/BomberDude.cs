using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberDude : MonoBehaviour
{
    public GameObject collisionEffect;
    public AudioSource explosionSource;
    Animator anim;
    private bool isChasing;
    private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= 5f)
        {
            isChasing = true;
        }
        else
        {
            isChasing = false;
        }

        anim.SetBool("isChasing", isChasing);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            ContactPoint2D contact = collision.GetContact(0);
            GameObject kaboom = Instantiate(collisionEffect, contact.point, Quaternion.identity);
            explosionSource.Play();
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            Destroy(kaboom, 1f);
            Destroy(gameObject, 1f);
        }
    }

    
}
