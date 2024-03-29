using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kryz.CharacterStats.Examples;
using UnityEngine.InputSystem;

public enum AlexanderSkill2JoystickMode
{
    FreeAngle,
    FourWays
}

public class AlexanderSkill2 : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    public float moveSpeed = 2;
    public float bulletLifetime = 5f; // Thời gian tồn tại của viên đạn
    public float shootDelay = 0.5f; // Khoảng thời gian delay trước khi đạn bắn ra

    private InputAction joystickAction;
    public AlexanderSkill2JoystickMode joystickMode = AlexanderSkill2JoystickMode.FreeAngle;
    private InputAction shootAction;
    private bool canShoot = true; // Biến kiểm tra có thể bắn đạn hay không

    private void OnEnable()
    {
        joystickAction.Enable();
        shootAction.Enable();
    }

    private void OnDisable()
    {
        joystickAction.Disable();
        shootAction.Disable();
    }

    private void Awake()
    {
        joystickAction = new InputAction("joystick", binding: "<Gamepad>/leftStick");
        joystickAction.performed += OnJoystickPerformed;

        shootAction = new InputAction("shoot", binding: "<Keyboard>/space");
        shootAction.performed += ctx => Shoot();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 joystickDirection = joystickAction.ReadValue<Vector2>();

        if (joystickDirection != Vector2.zero)
        {
            float angle = 0f;

            switch (joystickMode)
            {
                case AlexanderSkill2JoystickMode.FreeAngle:
                    angle = Mathf.Atan2(joystickDirection.y, joystickDirection.x) * Mathf.Rad2Deg;
                    break;
                case AlexanderSkill2JoystickMode.FourWays:
                    float x = joystickDirection.x;
                    float y = joystickDirection.y;

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x > 0)
                            angle = 0f;
                        else
                            angle = 180f;
                    }
                    else
                    {
                        if (y > 0)
                            angle = 90f;
                        else
                            angle = -90f;
                    }

                    break;
            }

            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    public void Shoot()
    {
        if (!gameObject.activeSelf || !canShoot)
        {
            // Nếu game object không active hoặc không thể bắn đạn, không thực hiện bắn đạn
            return;
        }

        // Tiếp tục thực hiện bắn đạn
        StartCoroutine(ShootWithDelay());
    }

    IEnumerator ShootWithDelay()
    {
        canShoot = false; // Vô hiệu hóa khả năng bắn đạn trong khoảng thời gian delay
        yield return new WaitForSeconds(shootDelay); // Chờ khoảng thời gian delay

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = bulletSpawnPoint.up * bulletSpeed;
        }
        Destroy(bullet, bulletLifetime);

        canShoot = true; // Kích hoạt lại khả năng bắn đạn
    }

    private void OnJoystickPerformed(InputAction.CallbackContext context)
    {
        // Xử lý sự kiện khi joystick được di chuyển
    }
}