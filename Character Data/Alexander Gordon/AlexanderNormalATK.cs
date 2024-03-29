using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Kryz.CharacterStats.Examples;

public class AlexanderNormalATK : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab1; // Đạn loại 1
    public GameObject bulletPrefab2; // Đạn loại 2
    public float bulletSpeed = 20f;
    public float maxDistance = 3f;
    public float skillCooldown = 0.75f;
    private float nextSkillTime = 0f;
    private bool isOnCooldown = false;
    public Animator animator;
    public Character DPS; // Tham chiếu đến PlayerStat

    public Button NormalButton;
    private float cooldownTimer = 0f; // Thời gian hồi chiêu

    private void Start()
    {
        NormalButton.onClick.AddListener(AGNormalATK);
    }

    private void Update()
    {
        // Cập nhật thời gian hồi chiêu
        cooldownTimer -= Time.deltaTime;

        if (isOnCooldown)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.U) && cooldownTimer <= 0f)
        {
            AGNormalATK(); // Gọi phương thức mà không cần StartCoroutine
            cooldownTimer = skillCooldown; // Đặt lại thời gian hồi chiêu
        }

        if (Input.GetButtonDown("Xbox_B_Button") && cooldownTimer <= 0f)
        {
            AGNormalATK(); // Gọi phương thức mà không cần StartCoroutine
            cooldownTimer = skillCooldown; // Đặt lại thời gian hồi chiêu
        }
    }

    private void AGNormalATK() // Không cần kiểu trả về
    {
        isOnCooldown = true;
        animator.SetTrigger("Attack");
    }

    // Phương thức này được gọi bởi Animation Event trong animation
    public void Shoot()
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

        // Xác định loại đạn dựa vào tỉ lệ xuất hiện
        GameObject bulletPrefabToUse = Random.Range(0f, 1f) <= 0.2f ? bulletPrefab2 : bulletPrefab1;

        GameObject bullet = Instantiate(bulletPrefabToUse, transform.position, Quaternion.identity);
        // Gọi phương thức Setup trong script Bullet và truyền thông tin từ DPS
        bullet.GetComponent<BulletAGNor>().Setup(shootVector.normalized * bulletSpeed, DPS);

        Destroy(bullet, 1f);
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