using System.Collections;
using UnityEngine;
using Kryz.CharacterStats.Examples;

public class KileSkill1 : MonoBehaviour
{
    public float damage = 1000f;
    public float knockbackForce = 10f;
    public float range = 5f;

    private bool isSkillActive = false;

    private void Start()
    {
        StartCoroutine(ActivateSkillAfterDelay());
    }

    private IEnumerator ActivateSkillAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        ActivateSkill();
    }

    private void Update()
    {
        CheckSkillStatus();
    }

    public void ActivateSkill()
    {
        if (!isSkillActive)
        {
            isSkillActive = true;
            StartCoroutine(PerformSkill());
        }
    }

    private IEnumerator PerformSkill()
    {
        yield return new WaitForSeconds(1.5f);
        DealDamageAndKnockback();
        isSkillActive = false;
    }

    private void DealDamageAndKnockback()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                collider.GetComponent<Character>().TakeDamage(damage);

                Vector2 knockbackDirection = (collider.transform.position - transform.position).normalized;
                collider.GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                Debug.Log("Dealing damage and knockback to player");

            }
        }
    }

    private void CheckSkillStatus()
    {
        // Check other conditions if needed
    }
}
