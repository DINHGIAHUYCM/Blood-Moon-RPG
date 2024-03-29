using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float chaseRadius = 5f; // Bán kính vòng tròn phạm vi tấn công
    public float safeDistance = 1f; // Khoảng cách an toàn để tránh mục tiêu
    public float moveSpeed = 3f; // Tốc độ di chuyển của quái
    public float attackDelay = 3f; // Thời gian trễ trước khi tấn công
    public GameObject bulletPrefab; // Prefab của viên đạn
    public Transform firePoint; // Vị trí bắn đạn

    private Animator animator; // Animator component của quái
    private bool isFacingRight = false; // Quái đang hướng về phải hay không
    private bool isAttacking = false; // Quái đang tấn công hay không

    private GameObject target; // Mục tiêu quái sẽ tấn công

    private void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("CheckAttack", attackDelay, attackDelay);
    }

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
        }

        if (target != null)
        {
            float distance = Vector3.Distance(target.transform.position, transform.position);

            // Kiểm tra nếu mục tiêu nằm trong phạm vi tấn công
            if (distance <= chaseRadius)
            {
                // Kiểm tra khoảng cách an toàn
                if (distance > safeDistance && !isAttacking)
                {
                    // Di chuyển quái đến mục tiêu
                    Vector3 moveDirection = (target.transform.position - transform.position).normalized;
                    transform.position += moveDirection * moveSpeed * Time.deltaTime;

                    // Kiểm tra và lật hướng của quái
                    if (moveDirection.x > 0 && !isFacingRight)
                    {
                        Flip();
                    }
                    else if (moveDirection.x < 0 && isFacingRight)
                    {
                        Flip();
                    }
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
        }
        else
        {
            // Ngừng di chuyển và tắt animation chase (nếu có)
            if (animator != null)
            {
                animator.SetBool("IsChasing", false);
            }
        }
    }

    private void FindTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void CheckAttack()
    {
        if (!isAttacking && target != null)
        {
            // Kích hoạt hành động tấn công
            isAttacking = true;
            animator.SetTrigger("Attack");

            // Tạo đạn và bắn
            Vector2 direction = target.transform.position - transform.position;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.LookRotation(Vector3.forward, direction));
            // Thực hiện logic bắn đạn (có thể thay đổi)

            // Đặt trạng thái tấn công về false sau một khoảng thời gian
            Invoke("ResetAttackState", attackDelay);
        }
    }

    private void ResetAttackState()
    {
        isAttacking = false;
    }

    private void Flip()
    {
        // Đảo ngược hướng của quái
        isFacingRight = !isFacingRight;

        // Lật sprite hoặc animation
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
