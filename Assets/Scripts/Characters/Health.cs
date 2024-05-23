using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth = 100f;
    public Transform healthBar;
    public bool isBoss = false;
    public RectTransform bossHealthBar;
    float originalBarSize;

    private void Start()
    {
        originalBarSize = bossHealthBar.sizeDelta.x;
        currentHealth = maxHealth;
        bossHealthBar.sizeDelta = new Vector2(originalBarSize * currentHealth / maxHealth, bossHealthBar.sizeDelta.y);
    }
    public void TakeDamage(float damage)
    {
        if(isBoss)
        {
            bossHealthBar.sizeDelta = new Vector2(originalBarSize * currentHealth / maxHealth, bossHealthBar.sizeDelta.y);
        }
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            print(currentHealth);
            Debug.Log("Health depleted!");
            Destroy(gameObject);
        }

        if(!isBoss)
        {
            float healthPercentage = (float)currentHealth / maxHealth;

        
            Vector3 newScale = new Vector3(healthPercentage, healthBar.localScale.y, healthBar.localScale.z);
            healthBar.localScale = newScale;
        }
    }
}
