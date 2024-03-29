using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kryz.CharacterStats.Examples;

public class JohnsonSkill1 : MonoBehaviour
{
    public Character PlayerCharacter;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public Animator animator;
    private bool isAttacking = false;
    private bool isThirdAttack = false;
    private float lockMovementTime = 0.5f;

    public bool IsActivated { get; private set; } = false; // New property to indicate if the skill is activated

    public Button Skill_1_Button;

    private float cooldownTimer = 0f;
    private float cooldownDuration = 4f;

        private float lastUsedTime = -Mathf.Infinity; // Initialize to negative infinity to indicate it's not on cooldown

    public bool IsOnCooldown()
    {
        return Time.time - lastUsedTime < cooldownTimer;
    }

    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel_JohnsonLyannar", 1);
        Skill_1_Button.onClick.AddListener(PerformSkill_1);
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (isAttacking)
        {
            return;
        }

        if (IsActivated) // Check if the skill is activated
        {
            // Perform any necessary actions when the skill is activated
        }

        // Kích hoạt bằng nút "I" trên bàn phím
        if (Input.GetKeyDown(KeyCode.I))
        {
            PerformSkill_1();
        }

        // Kích hoạt bằng nút A trên tay cầm Xbox
        if (Input.GetButtonDown("Xbox_A_Button"))
        {
            PerformSkill_1();
        }
    }

    private void PerformSkill_1()
    {
        if (cooldownTimer > 0)
        {
            // Debug.Log("Skill is on cooldown!");
            return;
        }

        if (isAttacking)
        {
            return;
        }

        float dps = PlayerCharacter.DPS.BaseValue;
        float critChance = PlayerCharacter.CritChance.BaseValue;
        float critDamageMultiplier = PlayerCharacter.CritDamage.BaseValue;

        int damage1 = CalculateDamage(dps * 0.8f, critChance, critDamageMultiplier);
        int damage2 = CalculateDamage(dps * 0.8f, critChance, critDamageMultiplier);

        cooldownTimer = cooldownDuration;
        StartCoroutine(Attack(damage1, damage2));
    }

    private int CalculateDamage(float baseDPS, float critChance, float critDamageMultiplier)
    {
        float finalDamage = baseDPS;

        if (Random.value < critChance)
        {
            finalDamage *= critDamageMultiplier;
            // Debug.Log("Critical hit!");
        }

        return Mathf.CeilToInt(finalDamage);
    }

    private IEnumerator Attack(int damage1, int damage2)
    {
        isAttacking = true;
        isThirdAttack = false;

        animator.SetTrigger("Skill1");
        isThirdAttack = true;
        StartCoroutine(LockMovementForTime(lockMovementTime));

        yield return new WaitForSeconds(0.15f);

        DamageEnemiesInAttackRange(damage1);

        yield return new WaitForSeconds(0.1f);

        if (!isAttacking)
        {
            yield break;
        }

        DamageEnemiesInAttackRange(damage2);

        yield return new WaitForSeconds(0.2f);

        if (!isAttacking)
        {
            yield break;
        }

        isThirdAttack = true;
        StartCoroutine(LockMovementForTime(lockMovementTime));

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;

        // Set IsActivated to true after the skill is performed
        IsActivated = true;
    }

    private IEnumerator LockMovementForTime(float time)
    {
        PlayerController2 playerController = GetComponent<PlayerController2>();
        if (playerController != null)
        {
            playerController.DisableControl();
        }

        yield return new WaitForSeconds(time);

        if (playerController != null)
        {
            playerController.EnableControl();
        }
    }

    private void DamageEnemiesInAttackRange(int damage)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
            if (enemyStat != null)
            {
                enemyStat.TakeDamage(damage);
            }
        }
    }
}
