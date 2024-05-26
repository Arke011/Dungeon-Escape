using UnityEngine;
using TMPro;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public GameObject healthBar;
    float originalBarSize;
    public AudioClip damageSound;
    public AudioClip deathSound;
    AudioSource source;

    public bool isDroplet;
    public bool isBoss;

    public TMP_Text bossHP;
    private Animator anim;

    private void Start()
    {
        originalBarSize = healthBar.transform.localScale.x;
        currentHealth = maxHealth;
        source = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        if (isBoss)
        {
            bossHP.text = currentHealth.ToString();
        }
    }

    public void TakeDamage(float damage)
    {
        source.PlayOneShot(damageSound);
        currentHealth -= damage;
        currentHealth = Mathf.Max(0f, currentHealth);

        if (isBoss)
        {
            bossHP.text = Mathf.RoundToInt(currentHealth).ToString();
        }

        if (currentHealth <= 0f)
        {
            HandleDeath();
        }

        UpdateHealthBar();
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Min(maxHealth, currentHealth);

        if (isBoss)
        {
            bossHP.text = Mathf.RoundToInt(currentHealth).ToString();
        }

        UpdateHealthBar();
    }

    

    private void HandleDeath()
    {
        source.PlayOneShot(deathSound);
        currentHealth = 0f;
        Debug.Log("Health depleted!");

        if (isBoss)
        {
            anim.SetTrigger("die");
            GetComponent<Boss>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        else if (isDroplet)
        {
            anim.SetBool("isMoving", false);
            anim.SetTrigger("die");
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<EnemyAI>().enabled = false;
        }
        else
        {
            anim.SetTrigger("die"); // Ensure die trigger is set for non-droplet, non-boss entities as well
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<EnemyAI>().enabled = false;
            RedMonster monstuh = GetComponent<RedMonster>();
            if (monstuh != null) { monstuh.enabled = false; }
        }

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

       
        if (isBoss)
        {
            Destroy(gameObject, 1.5f);
        }
        else
        {
            Destroy(gameObject, 0.5f);
        }
    }

    private void UpdateHealthBar()
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);
        Vector3 newScale = new Vector3(healthPercentage * originalBarSize, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.localScale = newScale;
    }
}
