using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kryz.CharacterStats.Examples;

public class JohnsonSkill2 : MonoBehaviour
{
    /*
    Kỹ năng 2 - Hỏa nộ
    -Tìm mục tiêu gần nhất
    -Xuyên qua mục tiêu

    Khi cường hóa sẽ gây gấp 3 lần sát thương tầm xa theo công vật lý
    */
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float maxDistance = 10f;
    public float skillCooldown = 5f;
    private float nextSkillTime = 0f;
    private bool isOnCooldown = false;
    private bool isActivated = false; // New property to indicate if the skill is activated
    public Animator animator;
    public Character DPS; // Tham chiếu đến PlayerStat

    private float lastUsedTime = -Mathf.Infinity; // Initialize to negative infinity to indicate it's not on cooldown

    public bool IsOnCooldown()
    {
        return Time.time - lastUsedTime < skillCooldown;
    }

    public Button skill2Button;
    private float cooldownTimer = 0f; // Thời gian hồi chiêu

    private void Start()
    {
        skill2Button.onClick.AddListener(UseSkill2Coroutine);
    }

    private void Update()
    {
        // Cập nhật thời gian hồi chiêu
        cooldownTimer -= Time.deltaTime;

        if (isOnCooldown)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.O) && cooldownTimer <= 0f)
        {
            UseSkill2Coroutine(); // Gọi phương thức mà không cần StartCoroutine
            cooldownTimer = skillCooldown; // Đặt lại thời gian hồi chiêu
        }

        if (Input.GetButtonDown("Xbox_X_Button") && cooldownTimer <= 0f)
        {
            UseSkill2Coroutine(); // Gọi phương thức mà không cần StartCoroutine
            cooldownTimer = skillCooldown; // Đặt lại thời gian hồi chiêu
        }
    }

    private void UseSkill2Coroutine()
    {
        isOnCooldown = true;
        animator.SetTrigger("Skill2");

        // Không cần StartCoroutine ở đây

        if (bulletSpawnPoint.gameObject.activeSelf)
        {
            Shoot();
        }

        // Đặt thời gian hồi chiêu để bắt đầu đếm lại
        cooldownTimer = skillCooldown;

        isOnCooldown = false;

        // Set IsActivated to true after the skill is performed
        isActivated = true;
    }

    void Shoot()
    {
        Vector3 targetPosition = FindClosestEnemy();
        Vector3 shootVector = Vector3.zero;

        if (targetPosition != Vector3.zero)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            shootVector = direction * maxDistance;
        }
        else
        {
            Movement();
        }

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        // Gọi phương thức Setup trong script Bullet và truyền thông tin từ DPS
        bullet.GetComponent<Bullet>().Setup(shootVector.normalized * bulletSpeed, DPS);

        Destroy(bullet, 2f);
    }

    void Movement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90 * h);
        }
        else if (v != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90 - 90 * v);
        }
    }

    Vector3 FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Vector3 closestPosition = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = enemy.transform.position;
            }
        }

        return closestPosition;
    }
}