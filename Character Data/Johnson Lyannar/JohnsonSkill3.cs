using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kryz.CharacterStats.Examples;

public class JohnsonSkill3 : MonoBehaviour
{
    public GameObject[] objectsToDisable;
    public GameObject[] objectsToEnable;
    public float skillRange = 5f;
    public Animator animator;
    public Character playerCharacter;
    public float damageMultiplier = 250f;
    public float damageMultiplierPerTick = 3f;

    private bool isActivated = false;
    private int damageTicks = 10;
    private float skillDuration = 10f;
    private float damageInterval = 0.5f;

    private float lastUsedTime = -Mathf.Infinity;

    public Button Skill_3_Button;

    private void Start()
    {
        Skill_3_Button.onClick.AddListener(UseSkill3);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UseSkill3();
        }

        if (Input.GetButtonDown("Xbox_Y_Button"))
        {
            UseSkill3();
        }
    }

    private void UseSkill3()
    {
        StartCoroutine(UseSkill3Coroutine());
    }

    private IEnumerator UseSkill3Coroutine()
    {
        animator.SetTrigger("Skill3");

        yield return new WaitForSeconds(0.15f);

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, skillRange);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null) // Kiểm tra Collider có tồn tại hay không
            {
                EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                if (enemyStat != null)
                {
                    int damage = Mathf.CeilToInt(playerCharacter.DPS.BaseValue * damageMultiplier);
                    enemyStat.TakeDamage(damage);
                }
            }
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }

        for (int i = 0; i < damageTicks; i++)
        {
            yield return new WaitForSeconds(damageInterval);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy != null) // Kiểm tra Collider có tồn tại hay không
                {
                    EnemyStat enemyStat = enemy.GetComponent<EnemyStat>();
                    if (enemyStat != null)
                    {
                        int damage = Mathf.CeilToInt(playerCharacter.DPS.BaseValue * damageMultiplierPerTick);
                        enemyStat.TakeDamage(damage);
                    }
                }
            }
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }
    }
}