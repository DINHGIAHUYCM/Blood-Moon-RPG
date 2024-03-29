using UnityEngine;

public class ChargeSkill : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float distance = 5f;
    public float interval = 5f;
    public float chargeTime = 2f; // Thời gian tăng tốc tấn công
    public float normalSpeed = 5f; // Tốc độ bình thường
    public float chargeSpeed = 10f; // Tốc độ tăng tốc

    private float timer;
    private bool isCharging;

    private void Start()
    {
        timer = interval;
        isCharging = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Tính toán hướng di chuyển của kẻ địch
            Vector3 direction = player.position - transform.position;
            direction.y = 0f;

            if (!isCharging) // Nếu không đang tăng tốc
            {
                // Di chuyển kẻ địch theo hướng đó với tốc độ bình thường
                transform.position += direction.normalized * normalSpeed * Time.deltaTime;

                // Kiểm tra khoảng cách giữa kẻ địch và player
                float dist = Vector3.Distance(transform.position, player.position);

                // Nếu khoảng cách nhỏ hơn hoặc bằng distance
                if (dist <= distance)
                {
                    // Gây sát thương cho player hoặc thực hiện hành động khác tùy vào game của bạn
                    Debug.Log("Player damaged");
                }

                // Kiểm tra xem có đủ thời gian để tăng tốc tấn công hay không
                if (timer <= -chargeTime)
                {
                    isCharging = true; // Bắt đầu tăng tốc tấn công
                    timer = interval; // Đặt lại timer
                }
            }
            else // Nếu đang tăng tốc
            {
                // Di chuyển kẻ địch theo hướng đó với tốc độ tăng tốc
                transform.position += direction.normalized * chargeSpeed * Time.deltaTime;

                // Kiểm tra khoảng cách giữa kẻ địch và player
                float dist = Vector3.Distance(transform.position, player.position);

                // Nếu khoảng cách nhỏ hơn hoặc bằng distance
                if (dist <= distance)
                {
                    // Gây sát thương cho player hoặc thực hiện hành động khác tùy vào game của bạn
                    Debug.Log("Player damaged");
                }

                // Kiểm tra xem đã đủ thời gian để tăng tốc tấn công hay chưa
                if (timer <= -(chargeTime + interval))
                {
                    isCharging = false; // Kết thúc tăng tốc tấn công
                    timer = interval; // Đặt lại timer
                }
            }
        }
    }
}
