using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public float chaseRadius = 5f; // Bán kính vòng tròn phạm vi đuổi theo
    public float safeDistance = 1f; // Khoảng cách an toàn để tránh mục tiêu
    public float moveSpeed = 3f; // Tốc độ di chuyển của quái

    public GameObject childObject; // Child object không bị lật ngược

    private Transform target; // Mục tiêu quái sẽ đuổi theo
    private Animator animator; // Animator component của quái
    private bool isFacingRight = false; // Quái đang hướng về phải hay không
    private Vector3 fixedMoveDirection; // Hướng di chuyển cố định

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FindClosestPlayer();

        if (target != null)
        {
            float distance = Vector3.Distance(target.position, transform.position);

            // Kiểm tra nếu mục tiêu nằm trong phạm vi đuổi theo
            if (distance <= chaseRadius)
            {
                // Kiểm tra khoảng cách an toàn
                if (distance > safeDistance)
                {
                    // Xác định điểm đến trung gian
                    Vector3 intermediatePoint = target.position + (transform.position - target.position).normalized * safeDistance;

                    // Di chuyển quái đến điểm đến trung gian
                    Vector3 moveDirection = (intermediatePoint - transform.position).normalized;
                    fixedMoveDirection = moveDirection; // Lưu hướng di chuyển cố định
                    transform.position += fixedMoveDirection * moveSpeed * Time.deltaTime;

                    // Kiểm tra và lật hướng của quái
                    if (fixedMoveDirection.x > 0 && !isFacingRight)
                    {
                        Flip();
                    }
                    else if (fixedMoveDirection.x < 0 && isFacingRight)
                    {
                        Flip();
                    }
                }
                else
                {
                    // Lùi ra xa nếu player áp sát
                    Vector3 moveDirection = (transform.position - target.position).normalized;
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;
                }

                // Kích hoạt animation chase (nếu có)
                if (animator != null)
                {
                    animator.SetBool("IsChasing", true);
                }
            }
            else
            {
                // Ngừng di chuyển và tắt animation chase (nếu có)
                if (animator != null)
                {
                    animator.SetBool("IsChasing", false);
                }
            }

            // Kích hoạt animation đứng yên nếu player đứng yên
            if (animator != null)
            {
                bool isIdle = target.GetComponent<Rigidbody2D>().velocity.magnitude < 0.1f;
                animator.SetBool("IsIdle", isIdle);

                // Cố định trục di chuyển khi animation Idle chạy
                if (isIdle)
                {
                    transform.position += fixedMoveDirection * moveSpeed * Time.deltaTime;
                }
            }
        }
    }

    private void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players != null && players.Length > 0)
        {
            float closestDistance = Mathf.Infinity;

            foreach (GameObject player in players)
            {
                float distance = Vector3.Distance(player.transform.position, transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = player.transform;
                }
            }
        }
        else
        {
            target = null;
        }
    }

    private void Flip()
    {
        // Đảo ngược hướng của quái
        isFacingRight = !isFacingRight;

        // Lật sprite hoặcanimation của parent object
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        // Đảo ngược hướng của child object
        scale = childObject.transform.localScale;
        scale.x *= -1;
        childObject.transform.localScale = scale;
    }
}