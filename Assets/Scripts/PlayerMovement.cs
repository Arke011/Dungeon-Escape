using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    Vector2 movement;

    private float dashingCooldown = 2f;
    private float dashTime = 0.2f;
    private bool canDash = true;
    private float dashingStrenght = 40f;
    private bool isDashing = false;
    private bool isFacingRight = true;

    private float horizontal, vertical;


    [SerializeField] private TrailRenderer rr;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        Flip();
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

        // Apply the dash velocity
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
