using System.Collections;
using System;
using UnityEngine;
using TMPro;
using BarthaSzabolcs.Tutorial_SpriteFlash;
using UnityEngine.UI;

public enum EnemyType
{
    Normal,
    Boss
}

public class EnemyStat : MonoBehaviour
{
    public Animator animator;
    public int maxHealth = 100;
    private int currentHealth;

    public TextMeshProUGUI healthText;
    private bool isDamaged = false;
    private float damageTimer = 0f;
    public float damageTime = 0.5f;
    public float deathDelay = 1f;

    public SimpleFlash flashEffect;

    public GameObject coinPrefab;
    public Transform coinSpawnPoint;

    public GameObject doomEffect;
    public float doomDelay = 3f;

    public EnemyType enemyType;

    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    public event Action<int> OnHealthChanged;

    private bool isDying = false;
    private float deathTimer = 0f;

    public TextMeshProUGUI hitDamageText; // Text hiển thị sát thương nhận vào

    public int Level = 1; // Level của Enemy
    public TextMeshProUGUI levelText;
    public int healthIncreasePerLevel = 2000; // Số máu tăng mỗi khi Level tăng

    public Slider healthSlider; // Slider hiển thị health bar

    [Space]
    public EnemyChase enemyChaseScript;

    private bool isNormalEnemy;

    private int GetCurrentMaxHealth()
    {
        return maxHealth + (Level - 1) * healthIncreasePerLevel;
    }

    void Start()
{
    currentHealth = GetCurrentMaxHealth(); // Sử dụng hàm GetCurrentMaxHealth() để lấy giá trị maxHealth ban đầu
    UpdateUI();
    if(levelText != null){
        levelText.text = "" + Level;
    }

    // Kiểm tra loại kẻ địch
    isNormalEnemy = enemyType == EnemyType.Normal;
}

    void Update()
    {
        if (isDamaged)
        {
            damageTimer += Time.deltaTime;

            if (damageTimer >= damageTime)
            {
                isDamaged = false;
                damageTimer = 0f;
            }
        }

        if (isDying)
        {
            deathTimer += Time.deltaTime;

            if (deathTimer >= deathDelay)
            {
                DestroyEnemy();
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (!isDamaged)
        {
            isDamaged = true;

            if (flashEffect != null)
            {
                flashEffect.Flash();
            }
        }

        UpdateUI();

        // Hiển thị sát thương nhận vào
        if (hitDamageText != null)
        {
            hitDamageText.text = "-" + amount;
            hitDamageText.gameObject.SetActive(true);
            StartCoroutine(HideHitDamageText());
        }

        if (currentHealth <= 0)
        {
            if (enemyType == EnemyType.Boss)
            {
                Die();
            }
            else
            {
                if (doomEffect != null)
                {
                    doomEffect.SetActive(true);
                }

                isDying = true;
                Invoke("DestroyEnemy", doomDelay);
            }

            OnHealthChanged?.Invoke(currentHealth);
        }
        else
        {
            OnHealthChanged?.Invoke(currentHealth);
        }
    }

    void Die()
    {
        if (enemyChaseScript != null)
{
    enemyChaseScript.enabled = false;
}
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        if (enemyType == EnemyType.Normal)
    {
        // Hủy đối tượng sau 2 giây
        animator.SetTrigger("Die");
        Invoke("DestroyEnemy", 2f);
    }

        if (enemyType != EnemyType.Boss || deathDelay == 0f)
        {
            DestroyEnemy();
        }
        else
        {
            isDying = true;
        }
    }

    void DestroyEnemy()
    {
        if (objectsToEnable != null)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                if (obj != null)
                {
                    obj.SetActive(true);
                }
            }
        }

        if (objectsToDisable != null)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                }
            }
        }

        if (coinPrefab != null && coinSpawnPoint != null)
        {
            Instantiate(coinPrefab, coinSpawnPoint.position, Quaternion.identity);
        }

        Destroy(gameObject);

        if (doomEffect != null)
        {
            Destroy(doomEffect);
        }
    }

    void UpdateUI()
    {
        if (currentHealth <= 0)
{
    if (enemyChaseScript != null)
    {
        enemyChaseScript.enabled = false;
         animator.SetTrigger("Die");
    }
}
        if (healthText != null)
        {
            healthText.text = currentHealth + " / " + GetCurrentMaxHealth(); // Sử dụng GetCurrentMaxHealth() để lấy giátrị maxHealth hiện tại
        }

        if (healthSlider != null)
        {
            float healthPercentage = (float)currentHealth / GetCurrentMaxHealth();
            healthSlider.value = healthPercentage;
        }
    }

    IEnumerator HideHitDamageText()
    {
        yield return new WaitForSeconds(0.25f);
        if (hitDamageText != null)
        {
            hitDamageText.gameObject.SetActive(false);
        }
    }
}