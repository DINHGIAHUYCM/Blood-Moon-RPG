using UnityEngine;
using Kryz.CharacterStats.Examples;

public class BulletAGNor : MonoBehaviour
{

    /*
        Đánh thường: Alexander sẽ sử dụng Sheild để gây sát thương lên kẻ địch

        Công thức: 1f
    */
    public float speed = 30f; 
    public float destroyTime = 1f;

    public int damage=50;
    private Rigidbody2D rb;
    public Character playerStat; // Tham chiếu đến PlayerStat

    public void Setup(Vector3 shootVector, Character playerStat)
    {
        this.playerStat = playerStat;
        rb = GetComponent<Rigidbody2D>();

        // Lưu dữ liệu vào bullet
        rb.velocity = shootVector.normalized * speed;
        float angle = Mathf.Atan2(shootVector.y, shootVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        Destroy(gameObject, destroyTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Enemy"))
    {
        float dpsValue = playerStat.SkillPower.Value;

        // Tính toán sát thương dựa trên thông tin từ playerStat
        
        damage = damage + Mathf.CeilToInt(dpsValue);
        
        collision.GetComponent<EnemyStat>().TakeDamage(damage);

        // Hủy đạn sau khi gây sát thương
        Destroy(gameObject);
    }
}


    // Các phần còn lại của script không cần thay đổi
}
