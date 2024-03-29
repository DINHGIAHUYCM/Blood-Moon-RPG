using UnityEngine;
using UnityEngine.UI;

namespace Kryz.CharacterStats.Examples
{
    public class E3Attack : MonoBehaviour
    {
        public Animator animator;
        public float attackInterval = 5f;
        public float attackRadius = 5f;
        public int baseDamage = 10;
        public int damageIncreasePerLevel = 5;
        public int Level = 1;

        private float lastAttackTime;
        public Slider cooldownSlider;

        private BoxCollider2D boxCollider;

        private int GetCurrentDamage()
        {
            return baseDamage + (Level - 1) * damageIncreasePerLevel;
        }

        private void Start()
        {
            lastAttackTime = Time.time;

            boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.enabled = false;

            InvokeRepeating("Attack", attackInterval, attackInterval);
        }

        private void Attack()
        {
            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, attackRadius);

            bool enemyInRange = false; // Kiểm tra nếu có kẻ địch trong phạm vi

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.CompareTag("Player"))
                {
                    Character playerCharacter = collider.GetComponent<Character>();
                    if (playerCharacter != null)
                    {
                        int damage = GetCurrentDamage();
                        playerCharacter.TakeDamage(damage);
                        enemyInRange = true;
                    }
                }
            }

            if (enemyInRange)
            {
                lastAttackTime = Time.time;
            }
            else
            {
                // Tạm dừng thời gian hồi đòn tấn công nếu không có kẻ địch trong phạm vi
                CancelInvoke("Attack");
                Invoke("ResumeAttack", attackInterval);
            }

            boxCollider.enabled = true;
            StartCoroutine(DisableColliderAfterDelay(0.5f));
        }

        private System.Collections.IEnumerator DisableColliderAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            boxCollider.enabled = false;
        }

        private void ResumeAttack()
        {
            InvokeRepeating("Attack", attackInterval, attackInterval);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRadius);
        }

        private void Update()
        {
            if (cooldownSlider != null)
            {
                float cooldownTimeRemaining = Mathf.Max(0f, attackInterval - (Time.time - lastAttackTime));
                float cooldownPercentage = cooldownTimeRemaining / attackInterval;
                cooldownSlider.value = 1f - cooldownPercentage;
            }
        }
    }
}