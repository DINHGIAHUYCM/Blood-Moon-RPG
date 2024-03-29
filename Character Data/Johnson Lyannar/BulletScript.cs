using UnityEngine;
using System.Collections;

namespace Kryz.CharacterStats.Examples
{
    public class BulletScript : MonoBehaviour
    {
        public float speed = 10f; // Tốc độ di chuyển của đạn
        public float destroyTime = 2f; // Thời gian hủy đạn mặc định

        private float multiple = 30f;

        private Rigidbody2D rb;
        public Character playerStat; // Tham chiếu đến PlayerStat

        public void Setup(Vector3 shootVector, Character playerStat)
        {
            this.playerStat = playerStat; // Gán giá trị từ PlayerStat

            rb = GetComponent<Rigidbody2D>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                // Trích xuất giá trị DPS từ playerStat và chuyển đổi thành kiểu float
                float dpsValue = playerStat.DPS.BaseValue;
                float critChance = playerStat.CritChance.BaseValue / 100f;
                float critDamage = playerStat.CritDamage.BaseValue;

                // Tính toán sát thương dựa trên thông tin từ playerStat và tỷ lệ chí mạng
                int damage = Mathf.CeilToInt(dpsValue * multiple);
                if (Random.value <= critChance)
                {
                    damage = Mathf.CeilToInt(damage + critDamage / 100f * damage);
                    //Debug.Log("Critical!:" +critChance +", Damage:" + damage +", Value:"+ Random.value);
                }
                else
                {
                    //Debug.Log("Not Crit!:" +critChance +", Damage:" + damage +", Value:"+ Random.value);
                }

                // Gây sát thương lên kẻ địch
                collision.GetComponent<EnemyStat>().TakeDamage(damage);
            }
        }
    }
}