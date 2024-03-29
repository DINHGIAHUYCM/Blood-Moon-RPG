using UnityEngine;
using Kryz.CharacterStats.Examples;

public class JohnsonBullet : MonoBehaviour
{
    public float speed = 10f; // tốc độ di chuyển của đạn
    public float destroyTime = 2f; // thời gian hủy đạn mặc định

    private Rigidbody2D rb;
    private Character playerStat; // Tham chiếu đến PlayerStat

    public void Setup(Vector3 shootVector, Character playerStat)
    {
        this.playerStat = playerStat; // Gán giá trị từ PlayerStat

        rb = GetComponent<Rigidbody2D>();

        // Lưu dữ liệu vào bullet
        rb.velocity = shootVector.normalized * speed;
        float angle = Mathf.Atan2(shootVector.y, shootVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Hủy đạn sau thời gian destroyTime
        Destroy(gameObject, destroyTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Trích xuất giá trị DPS từ playerStat và chuyển đổi thành kiểu float
            float dpsValue = playerStat.DPS.Value;

            // Tính toán sát thương dựa trên thông tin từ playerStat
            float damageMultiplier = Random.Range(1.0f, 2.0f);
            int damage = Mathf.CeilToInt(dpsValue * 5 * damageMultiplier);
            // Bạn có thể thêm tính toán sát thương crit ở đây nếu cần
            Debug.Log("Random Range: "+ damageMultiplier);

            // Gây sát thương lên kẻ địch
            collision.GetComponent<EnemyStat>().TakeDamage(damage);

            // Hủy đạn sau khi gây sát thương
            // Destroy(gameObject);
        }
    }
}