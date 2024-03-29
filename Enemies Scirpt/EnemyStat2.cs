using UnityEngine;
using TMPro;
using System.Collections;

public class EnemyStat2 : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public TextMeshProUGUI healthText;

    private bool isDead = false;

    private Animator animator;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();

        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead)
        {
            return;
        }

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateUI();
    }

    void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;

        // Kích hoạt animation chết
        animator.SetTrigger("Dead");

        // Chờ 1 giây trước khi hủy đối tượng game
        StartCoroutine(DestroyAfterDelay(1f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Hủy đối tượng game
        Destroy(gameObject);
    }

    void UpdateUI()
    {
        healthText.text = "Enemy HP: " + currentHealth + " / " + maxHealth;
    }
}
