using UnityEngine;

namespace Kryz.CharacterStats.Examples
{
    public class AGSkill2Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public float destroyTime = 2f;

        private Rigidbody2D rb;
        public Character playerCharacter;
        private Transform target;

        public void Setup(Vector3 shootVector, Character playerCharacter)
        {
            rb = GetComponent<Rigidbody2D>();
            this.playerCharacter = playerCharacter;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = Mathf.Infinity;
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    target = enemy.transform;
                }
            }

            if (target != null)
            {
                Vector2 direction = (target.position - transform.position).normalized;
                rb.velocity = direction * speed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
            else
            {
                // If there are no enemies, move in the shoot direction of the character
                Vector2 direction = new Vector2(shootVector.x, shootVector.y).normalized;
                rb.velocity = direction * speed;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            Destroy(gameObject, destroyTime);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                float damage = playerCharacter.Shield.BaseValue;
                int damageMultiplier = PlayerPrefs.GetInt("Alexander Gordon", 1);
                int damageInt = CalculateDamage(damage * damageMultiplier*5);
                collision.GetComponent<EnemyStat>().TakeDamage(damageInt);

                // Destroy(gameObject);
            }
        }

        private int CalculateDamage(float baseDPS)
        {
            float totalDPS = baseDPS;
            int damage = Mathf.CeilToInt(totalDPS);

            return damage;
        }
    }
}