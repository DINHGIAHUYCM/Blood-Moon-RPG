using UnityEngine;

public class BossSpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab của Enemy

    private GameObject currentEnemy;  // Enemy hiện tại đã được tạo

    private void OnEnable()
    {
        SpawnEnemy();
    }

    private void OnDisable()
    {
        DestroyEnemy();
    }

    private void SpawnEnemy()
    {
        if (currentEnemy == null)
        {
            // Tạo một Enemy mới tại vị trí của GameObject này
            currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }

    private void DestroyEnemy()
    {
        if (currentEnemy != null)
        {
            // Hủy Enemy hiện tại nếu nó tồn tại
            Destroy(currentEnemy);
            currentEnemy = null;
        }
    }
}