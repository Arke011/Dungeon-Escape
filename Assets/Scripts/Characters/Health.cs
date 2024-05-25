using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public GameObject healthBar;
    float originalBarSize;
    public AudioClip damageSound;
    public AudioClip deathSound;
    AudioSource source;
    

    private void Start()
    {
        originalBarSize = healthBar.transform.localScale.x;
        currentHealth = maxHealth;
        source = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        source.PlayOneShot(damageSound);
        currentHealth -= damage;
        

        if (currentHealth <= 0f)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            source.PlayOneShot(deathSound);
            currentHealth = 0f;
            Debug.Log("Health depleted!");
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            RedMonster monstuh = GetComponent<RedMonster>();
            if (monstuh != null) { monstuh.enabled = false; }
            
            
            Destroy(gameObject, 0.5f);
        }

        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);
        Vector3 newScale = new Vector3(healthPercentage * originalBarSize, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.localScale = newScale;
    }
}
