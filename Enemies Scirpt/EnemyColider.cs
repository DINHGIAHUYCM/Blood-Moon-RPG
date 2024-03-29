using UnityEngine;

public class EnemyColider : MonoBehaviour
{
    public LayerMask wallLayer; // Layer cho tường
    public float speed = 5f; // Tốc độ di chuyển của đối tượng Enemy

    public Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy tham chiếu đến Rigidbody2D khi bắt đầu
    }

    private void Update()
    {
        // Kiểm tra xem có va chạm với tường không
        Collider2D wallHit = Physics2D.OverlapBox(transform.position, transform.localScale, 0, wallLayer);

        // Nếu có va chạm với tường, xử lý nó ở đây (ví dụ: dừng di chuyển)
        if (wallHit != null)
        {
            // Xử lý va chạm với tường ở đây
            // Ví dụ: dừng di chuyển
            rb.velocity = Vector2.zero;
        }
        else
        {
            // Không có va chạm, tiếp tục di chuyển
            // Ví dụ: di chuyển theo hướng hiện tại
            rb.velocity = transform.right * speed;
        }
    }
}
