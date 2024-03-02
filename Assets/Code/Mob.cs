//reference: https://www.youtube.com/watch?v=_lREXfAMUcE
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mob : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float moveDistance = 5f;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;
    [SerializeField] float health, maxHealth = 3f;

    [SerializeField] HealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<HealthBar>();
    }

    public float scaleFactor = 1.5f; 
    public float scaleDuration = 1f;
    private bool isAttacking = false;

    public GameObject fireballPrefab; 

    private void Start()
    {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        startPosition = transform.position;
        targetPosition = startPosition + Vector3.right * moveDistance;
    }

    private void StartAttack()
    {
        StartCoroutine(PerformAttack());
    }

    // the mob will attack when it reaches both eages of their routes
    IEnumerator PerformAttack()
    {
        isAttacking = true;

        Vector3 originalScale = transform.localScale;
        // Equally scaled in all directions
        Vector3 targetScale = originalScale * scaleFactor;

        // make it bug
        float timer = 0;
        while (timer <= scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, timer / scaleDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // recover to normal size
        timer = 0;
        while (timer <= scaleDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, timer / scaleDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 spawnPosition = transform.position; // use mob's current position
        spawnPosition += -transform.right * 0.5f; // fireball start from left

        if (fireballPrefab != null)
        {
            Quaternion fireballRotation = Quaternion.Euler(0, 0, 0); 
            GameObject fireballInstance = Instantiate(fireballPrefab, spawnPosition, fireballRotation); // get fireball instance

            // ignore collision between fireball and mob
            Physics2D.IgnoreCollision(fireballInstance.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }



        isAttacking = false;
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;
        healthBar.UpdateHealthBar(health, maxHealth);
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        // if attacking, stay still
        if (!isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 1f)
            {
                if (movingRight)
                {
                    targetPosition = startPosition - Vector3.right * moveDistance;
                }
                else
                {
                    targetPosition = startPosition + Vector3.right * moveDistance;
                }
                movingRight = !movingRight; 

                StartAttack();
            }
        }
    }



    // attack players
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(1);
        }

        if (collision.gameObject.GetComponent<Projectile>())
        {
            Destroy(gameObject);
        }
    }




}



  