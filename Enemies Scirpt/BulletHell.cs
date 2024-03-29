// Bullet

using Kryz.CharacterStats.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletHell : MonoBehaviour
{
    public float bulletLife = 1f;  // Defines how long before the bullet is destroyed
    public float rotation = 0f;
    public float speed = 1f;
    public int damage = 10; // Sát thương của viên đạn


    private Vector2 spawnPoint;
    private float timer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }


    // Update is called once per frame
    void Update()
    {
        if(timer > bulletLife) Destroy(this.gameObject);
        timer += Time.deltaTime;
        transform.position = Movement(timer);
    }


    private Vector2 Movement(float timer) {
        // Moves right according to the bullet's rotation
        float x = timer * speed * transform.right.x;
        float y = timer * speed * transform.right.y;
        return new Vector2(x+spawnPoint.x, y+spawnPoint.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Xử lý va chạm với đối tượng
        if (collision.CompareTag("Player"))
        {
            Character playerCharacter = collision.GetComponent<Character>();
            if (playerCharacter != null)
            {
                playerCharacter.TakeDamage(damage); // Gọi hàm TakeDamage của Character để gây sát thương
            }

            // Hủy viên đạn sau khi va chạm với player
            Destroy(gameObject);
        }
    }
}

