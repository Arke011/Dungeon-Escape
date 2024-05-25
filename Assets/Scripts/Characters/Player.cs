using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Player : MonoBehaviour
{
    public Animator anim;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    Vector2 movement;

    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public TMP_Text hpTXT;

    private float dashingCooldown = 2f;
    private float dashTime = 0.2f;
    private bool canDash = true;
    private float dashingStrenght = 40f;
    private bool isDashing = false;
    private bool isFacingRight = true;
    public Image dashTimerImage;

    private float horizontal, vertical;

    private bool isCooldown;

    [SerializeField] private TrailRenderer rr;
    AudioSource source;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isCooldown = false;
        currentHealth = maxHealth;
        hpTXT.text = currentHealth.ToString();
        source = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (isCooldown)
        {
            dashTimerImage.fillAmount -= 1 / dashingCooldown * Time.deltaTime;
            if (dashTimerImage.fillAmount <= 0)
            {
                dashTimerImage.fillAmount = 1;
                isCooldown = false;

            }
        }

        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        float spid = Mathf.Max(Mathf.Abs(horizontal), Mathf.Abs(vertical));
        anim.SetFloat("Speed", spid);
        

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            isCooldown = true;
            StartCoroutine(Dash());
        }

        Flip();
    }

    public void TakeDamage(float damage)
    {
        source.Play();
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            print(currentHealth);
            Debug.Log("Health depleted!");
            Destroy(gameObject);
        }

        hpTXT.text = currentHealth.ToString();
    }

    private void Flip()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        Vector3 lookDirection = mousePos - transform.position;
        lookDirection.y = 0;

        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        if (angle > 90 || angle < -90)
        {
            if (isFacingRight)
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingRight = false;
            }
        }
        else
        {
            if (!isFacingRight)
            {
                transform.Rotate(0f, 180f, 0f);
                isFacingRight = true;
            }
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        //rb.velocity = new Vector2(transform.localScale.x * dashingStrenght, 0f);
        Vector2 dashDirection = new Vector2(horizontal, vertical).normalized;

        
        rb.velocity = dashDirection * dashingStrenght;
        rr.emitting = true;
        yield return new WaitForSeconds(dashTime);
        rr.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);
    }
}
