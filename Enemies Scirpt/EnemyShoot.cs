using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;
    public float maxDistance = 100f;
    public float fireRate = 0.5f;
    public float shootingRange = 10f;
    public LineRenderer lineRenderer;
    private float fireTimer = 0.0f;

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update()
    {
        // Tính toán thời gian giữa các frame
        fireTimer += Time.deltaTime;

        // Nếu thời gian giữa các lần bắn đủ lớn
        if (fireTimer >= fireRate)
        {
            // Reset timer và bắn
            fireTimer = 0.0f;
            EShoot();
        }
    }

    void EShoot()
    {
        Vector3 targetPosition = FindClosestPlayer();
        float distanceToTarget = Vector3.Distance(targetPosition, transform.position);

        // Kiểm tra nếu player nằm ngoài khoảng cách bắn
        if (targetPosition == Vector3.zero || distanceToTarget > shootingRange)
        {
            return; // Không bắn
        }

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // Tính toán hướng bắn và lượng vector cần dịch chuyển
        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Mathf.Min(distanceToTarget, maxDistance);
        Vector3 shootVector = direction * distance;

        // Bắn đạn theo hướng tính toán được
        bullet.GetComponent<Rigidbody2D>().velocity = shootVector.normalized * bulletSpeed;

        // Hiển thị đường đỏ ngắm vào player
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, bullet.transform.position);
        lineRenderer.SetPosition(1, targetPosition);

        Destroy(bullet, 2f); // Đạn tồn tại trong 2 giây

        // Ẩn đường đỏ sau khi bắn xong
        StartCoroutine(HideLineRenderer());
    }

    IEnumerator HideLineRenderer()
    {
        yield return new WaitForSeconds(0.2f); // Thời gian chờ trước khi ẩn đường đỏ (có thể điều chỉnh)
        lineRenderer.enabled = false;
    }

    Vector3 FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Vector3 closestPosition = Vector3.zero;
        float closestDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = player.transform.position;
            }
        }

        return closestPosition;
    }
}
