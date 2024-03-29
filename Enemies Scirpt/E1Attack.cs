using System.Diagnostics;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class E1Attack : MonoBehaviour
{
    public float chaseRadius = 5f;
    public float safeDistance = 1f;
    public float moveSpeed = 3f;
    public float attackDelay = 3f;
    public float bulletSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public Slider attackTimeSlider;

    private Animator animator;
    private bool isFacingRight = false;
    private bool isAttacking = false;
    private float attackTimer = 0f;

    private List<GameObject> players = new List<GameObject>();
    private GameObject currentTarget;

    private void Start()
    {
        animator = GetComponent<Animator>();
        FindPlayers();
    }

    private void Update()
    {
        FindPlayers();

        if (players.Count > 0)
        {
            currentTarget = FindClosestPlayer();

            if (currentTarget != null)
            {
                float distance = Vector3.Distance(currentTarget.transform.position, transform.position);
                Vector3 attackDirection = currentTarget.transform.position - transform.position;

                if (distance <= chaseRadius)
                {
                    if (distance > safeDistance && !isAttacking)
                    {
                        Vector3 moveDirection = (currentTarget.transform.position - transform.position).normalized;
                        transform.position += moveDirection * moveSpeed * Time.deltaTime;

                        if (moveDirection.x > 0 && !isFacingRight)
                        {
                            Flip();
                        }
                        else if (moveDirection.x < 0 && isFacingRight)
                        {
                            Flip();
                        }
                    }
                    else if (!isAttacking)
                    {
                        CheckAttack(attackDirection, distance);
                        isAttacking = true;
                    }
                }
                else
                {
                    if (animator != null)
                    {
                        animator.SetBool("IsChasing", false);
                    }
                }
            }
        }

        if (isAttacking)
        {
            attackTimer -= Time.deltaTime;
            attackTimeSlider.value = attackTimer / attackDelay;

            if (attackTimer <= 0f)
            {
                isAttacking = false;
                attackTimeSlider.value = 1f;
            }
        }
    }

    private void FindPlayers()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players.Clear();
        players.AddRange(playerObjects);
    }

    private GameObject FindClosestPlayer()
    {
        GameObject closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestPlayer = player;
                closestDistance = distance;
            }
        }

        return closestPlayer;
    }

    private void CheckAttack(Vector3 attackDirection, float distance)
    {
        if (!isAttacking && currentTarget != null)
        {
            if (distance <= chaseRadius)
            {
                if (animator != null)
                {
                    if (Mathf.Abs(attackDirection.x) > Mathf.Abs(attackDirection.y))
                    {
                        if (attackDirection.x < 0.0)
                        {
                            animator.SetTrigger("AttackLeft");
                        }
                        else if (attackDirection.x >= 0.0)
                        {
                            animator.SetTrigger("AttackLeft");
                        }
                    }
                    else
                    {
                        if (attackDirection.y > 0.0)
                        {
                            animator.SetTrigger("AttackUp");
                        }
                        else if (attackDirection.y <= 0.0)
                        {
                            animator.SetTrigger("AttackDown");
                        }
                    }
                }

                if (animator != null)
                {
                    animator.SetBool("IsChasing", true);
                }

                if (!IsReloading())
                {
                    isAttacking = true;
                    attackTimer = attackDelay;
                    attackTimeSlider.value = 0f;

                    Vector2 direction = currentTarget.transform.position - firePoint.position;
                    GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                    Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                    if (bulletRigidbody != null)
                    {
                        bulletRigidbody.velocity = direction.normalized * bulletSpeed;
                    }
                }
            }
        }
    }

    private bool IsReloading()
    {
        return false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
}