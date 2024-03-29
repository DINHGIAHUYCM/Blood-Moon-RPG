using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kryz.CharacterStats.Examples;

public class JohnsonMeleeATK : MonoBehaviour
{
    public Character PlayerCharacter;
    public float attackRange = 1f;
    public LayerMask enemyLayer;
    public Animator animator;
    private bool isAttacking = false;
    private bool canMove = true;
    private bool isThirdAttack = false;
    private float lockMovementTime = 0.5f;

    public Button normalAttackButton;

    private void Start()
    {
        normalAttackButton.onClick.AddListener(PerformSkill);
    }

    private void Update()
    {
        if (isAttacking || !canMove)
        {
            return;
        }
        // Kích hoạt bằng nút "U" trên bàn phím
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformSkill();
        }

        // Kích hoạt bằng nút X trên tay cầm Xbox
        if (Input.GetButtonDown("Xbox_B_Button"))
        {
            PerformSkill();
        }
    }

    private void PerformSkill()
    {
        if (isAttacking)
        {
            return;
        }

        float dps = PlayerCharacter.DPS.BaseValue;
        float critChance = PlayerCharacter.CritChance.BaseValue;
        float critDamageMultiplier = PlayerCharacter.CritDamage.BaseValue;

        int damage1 = CalculateDamage(dps * 0.6f, critChance, critDamageMultiplier);
        int damage2 = CalculateDamage(dps * 0.8f, critChance, critDamageMultiplier);
        int damage3 = CalculateDamage(dps * 1.2f, critChance, critDamageMultiplier);

        StartCoroutine(Attack(damage1, damage2, damage3));
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

    private IEnumerator Attack(int damage1, int damage2, int damage3)
    {
        isAttacking = true;
        isThirdAttack = false;

        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.15f);

        DamageEnemiesInAttackRange(damage1);

        yield return new WaitForSeconds(0.25f);

        if (!isAttacking)
        {
            yield break;
        }

        DamageEnemiesInAttackRange(damage2);

        yield return new WaitForSeconds(0.4f);

        if (!isAttacking)
        {
            yield break;
        }

        isThirdAttack = true;
        LockMovementForTime(lockMovementTime); // Gọi hàm LockMovementForTime
        DamageEnemiesInAttackRange(damage3);

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;
        canMove = true; // Mở khóa di chuyển sau khi kết thúc chiêu
    }

    private void LockMovementForTime(float time)
    {
        canMove = false; // Khóa di chuyển
        Invoke("UnlockMovement", time); // Gọi hàm UnlockMovement sau thời gian time
    }

    private void UnlockMovement()
    {
        canMove = true; // Mở khóa di chuyển
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
