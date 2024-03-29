using UnityEngine;

namespace Kryz.CharacterStats.Examples
{
    public class BulletController : MonoBehaviour
    {
        public float speed = 10f; // Tốc độ di chuyển của viên đạn
        public int baseDamage = 10; // Sát thương cơ bản của viên đạn
        public int damageIncreasePerLevel = 5; // Sát thương tăng mỗi khi cấp độ đạn tăng

        private Vector3 direction; // Hướng di chuyển của viên đạn

        public int Level = 1; // Cấp độ đạn

        private int GetCurrentDamage()
        {
            return baseDamage + (Level - 1) * damageIncreasePerLevel;
        }

        private void Update()
        {
            // Di chuyển viên đạn theo hướng đã được xác định
            transform.position += direction * speed * Time.deltaTime;
        }

        public void SetDirection(Vector3 newDirection)
        {
            // Xác định hướng di chuyển của viên đạn
            direction = newDirection.normalized;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Xử lý va chạm với đối tượng
            if (collision.CompareTag("Player"))
            {
                Character playerCharacter = collision.GetComponent<Character>();
                if (playerCharacter != null)
                {
                    int damage = GetCurrentDamage();
                    playerCharacter.TakeDamage(damage); // Gọi hàm TakeDamage của Character để gây sát thương
                }

                // Hủy viên đạn sau khi va chạm với player
                Destroy(gameObject);
            }
        }
    }
}