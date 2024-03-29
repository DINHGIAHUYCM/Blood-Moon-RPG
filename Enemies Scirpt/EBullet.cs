// using UnityEngine;

// public class EBullet : MonoBehaviour
// {
//     public float speed = 10f; // tốc độ di chuyển của đạn
//     public int damage = 10; // lượng sát thương của đạn
//     public float destroyTime = 2f; // thời gian hủy đạn mặc định

//     private Rigidbody2D rb;
//     private Transform target; // kẻ địch gần nhất

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();

//         // tìm kẻ địch gần nhất
//         GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
//         float closestDistance = Mathf.Infinity;
//         foreach (GameObject player in players)
//         {
//             float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
//             if (distanceToPlayer < closestDistance)
//             {
//                 closestDistance = distanceToPlayer;
//                 target = player.transform;
//             }
//         }
//     }

//     void FixedUpdate()
//     {
//         if (target != null)
//         {
//             // // di chuyển đạn theo hướng của kẻ địch gần nhất
//             // Vector2 direction = (target.position - transform.position).normalized;
//             // rb.velocity = direction * speed;

//             // // xoay đạn để hướng về kẻ địch
//             // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
//             // transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
//         }
//         else
//         {
//             // nếu không tìm thấy kẻ địch gần nhất thì di chuyển thẳng theo hướng hiện tại
//             rb.velocity = transform.right * speed;
//         }
//     }

//     void OnTriggerEnter2D(Collider2D collision)
//     {
//         if (collision.CompareTag("Player"))
//         {
//             // gây sát thương lên kẻ địch
//             collision.GetComponent<PlayerStat>().TakeDamage(damage);

//             // hủy đạn sau khi chạm kẻ địch
//             Destroy(gameObject);
//         }
//     }

//     void Update()
//     {
//         // hủy đạn sau thời gian destroyTime
//         Destroy(gameObject, destroyTime);
//     }
// }


