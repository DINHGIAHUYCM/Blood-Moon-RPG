using UnityEngine;
using System.Collections;

public class EnemiesMovement : MonoBehaviour
{
    public float speed = 5f; // tốc độ di chuyển của kẻ địch
    private Transform player; // tham chiếu đến player

    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // tìm player theo tag "Player"
    }

    void Update ()
    {
        // tính toán hướng đi tới player
        Vector3 direction = player.position - transform.position;
        direction.y = 0; // giữ kẻ địch ở cùng một mặt phẳng với player
        direction.Normalize(); // chuẩn hóa hướng đi

        // di chuyển kẻ địch theo hướng tính toán được
        transform.position += direction * speed * Time.deltaTime;
    }
}
