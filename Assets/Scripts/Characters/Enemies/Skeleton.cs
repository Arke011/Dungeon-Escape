using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public AudioSource slashSource;
    Animator anim;
    private bool isAttacking;
    public float attackCD;
    public float startAttackCD;

    void Start()
    {
        anim = GetComponent<Animator>();
        startAttackCD = attackCD;
    }

    void Update()
    {
        if (attackCD > 0)
        {
            attackCD -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = true;
            anim.SetBool("isAttacking", isAttacking);
            Attack();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isAttacking = false;
            anim.SetBool("isAttacking", isAttacking);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && attackCD <= 0)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (attackCD <= 0)
        {
            slashSource.Play();
            attackCD = startAttackCD;
        }
    }
}
