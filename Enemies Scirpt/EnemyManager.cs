using UnityEngine;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemies;
    public string enemyTag = "Enemy";
    public AudioSource backgroundMusic;
    public GameObject winPanel;
    public TextMeshProUGUI enemyCountText; // Tham chiếu đến Text Mesh Pro

    private bool isGameOver = false;

    private void Start()
    {
        // Lấy tất cả các đối tượng có tag "Enemy"
        enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Khởi tạo Text Mesh Pro với số lượng kẻ địch ban đầu
        UpdateEnemyCountText();
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        int remainingEnemyCount = 0;

        // Đếm số lượng đối tượng "Enemy" còn lại
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null) // Kiểm tra xem đối tượng có bị tiêu diệt chưa
            {
                remainingEnemyCount++;
            }
        }

        if (remainingEnemyCount == 0)
        {
            GameOver();
        }

        // Cập nhật Text Mesh Pro với số kẻ địch còn lại
        UpdateEnemyCountText();
    }

    // Hàm này được gọi khi một đối tượng "enemy" bị tiêu diệt
    public void DestroyEnemy(GameObject enemy)
    {
        if (enemy != null)
        {
            Destroy(enemy);
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        backgroundMusic.Stop();
        // Thay đổi nhạc nền
        // backgroundMusic.clip = yourWinClip;
        // backgroundMusic.Play();
        winPanel.SetActive(true);
        Debug.Log("Winning!");
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
        {
            int remainingEnemyCount = 0;

            // Đếm số lượng đối tượng "Enemy" còn lại
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null) // Kiểm tra xem đối tượng có bị tiêu diệt chưa
                {
                    remainingEnemyCount++;
                }
            }

            enemyCountText.text = "Enemies remaining: " + remainingEnemyCount;
        }
    }
}
